using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Orders;
using TomatoPizza.Data.Entities;
using TomatoPizza.Data.Identity;
using TomatoPizza.Data.Repos;

namespace TomatoPizza.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public OrderService(IOrderRepo orderRepo, UserManager<AppUser> userManager, AppDbContext context)
        {
            _orderRepo = orderRepo;
            _userManager = userManager;
            _context = context;
        }

        public async Task<OrderReadDTO> CreateOrderAsync(string userId, OrderCreateDTO dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId)
                       ?? throw new Exception("User not found.");

            var selectedDishes = await _context.Dishes
                .Where(d => dto.DishIds.Contains(d.Id))
                .ToListAsync();

            if (!selectedDishes.Any())
                throw new Exception("No valid dishes selected.");

            decimal originalTotal = selectedDishes.Sum(d => d.Price);
            decimal total = originalTotal;
            decimal discountAmount = 0;
            bool usedBonus = false;
            string cheapestDishName = "";
            int originalBonus = user.BonusPoints;

            // Check if user is Premium
            var isPremium = await _userManager.IsInRoleAsync(user, "PremiumUser");

            if (isPremium)
            {
                // 20% discount if 3+ dishes
                if (selectedDishes.Count >= 3)
                {
                    discountAmount += total * 0.2m;
                    total -= discountAmount;
                }

                // Use bonus points to get cheapest dish free
                if (dto.UseBonusPoints && user.BonusPoints >= 100)
                {
                    decimal cheapestDish = selectedDishes.Min(d => d.Price);
                    cheapestDishName = selectedDishes.OrderBy(d => d.Price).First().Name;

                    discountAmount += cheapestDish;
                    total -= cheapestDish;
                    user.BonusPoints -= 100;
                    usedBonus = true;
                }

                // Add bonus points for dishes
                user.BonusPoints += selectedDishes.Count * 10;
                await _userManager.UpdateAsync(user);
            }

            var newOrder = new Order
            {
                UserId = user.Id,
                Dishes = selectedDishes,
                TotalPrice = total,
                UsedBonus = usedBonus
            };

            var saved = await _orderRepo.CreateAsync(newOrder);

            // Prepare bonus info message
            string bonusInfo = usedBonus
                ? $"You had {originalBonus} bonus points and used 100 to get a free pizza: '{cheapestDishName}'! You now have {user.BonusPoints} bonus points remaining."
                : isPremium
                    ? user.BonusPoints < 100
                        ? $"You currently have {user.BonusPoints} bonus points. Earn {100 - user.BonusPoints} more to unlock a bonus discount!"
                        : $"You have {user.BonusPoints} bonus points. Great job!"
                    : $"You are a RegularUser with {user.BonusPoints} bonus points. Order more to earn rewards or get promoted to PremiumUser for extra discounts!";


            //if (isPremium)
            //{
            //    if (usedBonus)
            //    {
            //        bonusInfo = $"You had {originalBonus} bonus points and used 100 to get a free pizza: '{cheapestDishName}'!";
            //    }
            //    else if (user.BonusPoints < 100)
            //    {
            //        bonusInfo = $"You currently have {user.BonusPoints} bonus points. Earn {100 - user.BonusPoints} more to unlock a bonus discount!";
            //    }
            //    else
            //    {
            //        bonusInfo = $"You have {user.BonusPoints} bonus points. Great job!";
            //    }
            //}

            return new OrderReadDTO
            {
                Id = saved.Id,
                CreatedAt = saved.CreatedAt,
                OriginalTotalPrice = originalTotal,
                DiscountApplied = discountAmount,
                TotalPrice = saved.TotalPrice,
                UsedBonus = saved.UsedBonus,
                Status = saved.Status,
                Dishes = saved.Dishes.Select(d => new OrderDishDTO
                {
                    Name = d.Name,
                    Price = d.Price
                }).ToList(),
                BonusInfo = bonusInfo
            };
        }



        //public async Task<OrderReadDTO> CreateOrderAsync(string userId, OrderCreateDTO dto)
        //{
        //    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId)
        //               ?? throw new Exception("User not found.");

        //    var selectedDishes = await _context.Dishes
        //        .Where(d => dto.DishIds.Contains(d.Id))
        //        .ToListAsync();

        //    if (!selectedDishes.Any())
        //        throw new Exception("No valid dishes selected.");

        //    decimal total = selectedDishes.Sum(d => d.Price);
        //    bool usedBonus = false;
        //    decimal originalTotal = selectedDishes.Sum(d => d.Price);
        //    decimal discountAmount = 0;

        //    // Premium logic
        //    var isPremium = await _userManager.IsInRoleAsync(user, "PremiumUser");

        //    if (isPremium && selectedDishes.Count >= 3)
        //    {
        //        discountAmount += originalTotal * 0.2m;  // 20% discount
        //        total = originalTotal - discountAmount;
        //    }

        //    if (isPremium && dto.UseBonusPoints && user.BonusPoints >= 100)
        //    {
        //        decimal cheapestDish = selectedDishes.Min(d => d.Price);
        //        discountAmount += cheapestDish;
        //        total -= cheapestDish;
        //        user.BonusPoints -= 100;
        //        usedBonus = true;
        //    }

        //    // Save and return
        //    var saved = await _orderRepo.CreateAsync(new Order
        //    {
        //        UserId = user.Id,
        //        Dishes = selectedDishes,
        //        TotalPrice = total,
        //        UsedBonus = usedBonus
        //    });

        //    return new OrderReadDTO
        //    {
        //        Id = saved.Id,
        //        CreatedAt = saved.CreatedAt,
        //        OriginalTotalPrice = originalTotal,
        //        DiscountApplied = discountAmount,
        //        TotalPrice = saved.TotalPrice,
        //        UsedBonus = saved.UsedBonus,
        //        Status = saved.Status,
        //        Dishes = saved.Dishes.Select(d => new OrderDishDTO
        //        {
        //            Name = d.Name,
        //            Price = d.Price
        //        }).ToList(),
        //        BonusInfo = user.BonusPoints < 100
        //            ? $"You currently have {user.BonusPoints} bonus points. Earn {100 - user.BonusPoints} more to unlock a bonus discount!"
        //            : $"You have {user.BonusPoints} bonus points. Great job!"
        //    };


        //    //if (isPremium)
        //    //{
        //    //    // Apply discount if 3+ dishes
        //    //    if (selectedDishes.Count >= 3)
        //    //        total *= 0.8m; // 20% off

        //    //    // Apply bonus if eligible
        //    //    if (dto.UseBonusPoints && user.BonusPoints >= 100)
        //    //    {
        //    //        total = Math.Max(0, total - selectedDishes.Min(d => d.Price));
        //    //        user.BonusPoints -= 100;
        //    //        usedBonus = true;
        //    //    }

        //    //    // Add points for each dish
        //    //    user.BonusPoints += selectedDishes.Count * 10;
        //    //    await _userManager.UpdateAsync(user);
        //    //}

        //    //var newOrder = new Order
        //    //{
        //    //    UserId = user.Id,
        //    //    Dishes = selectedDishes,
        //    //    TotalPrice = total,
        //    //    UsedBonus = usedBonus
        //    //};

        //    //var saved = await _orderRepo.CreateAsync(newOrder);

        //    //return new OrderReadDTO
        //    //{
        //    //    Id = saved.Id,
        //    //    CreatedAt = saved.CreatedAt,
        //    //    TotalPrice = saved.TotalPrice,
        //    //    UsedBonus = saved.UsedBonus,
        //    //    Status = saved.Status,
        //    //    Dishes = saved.Dishes.Select(d => new OrderDishDTO
        //    //    {
        //    //        Name = d.Name,
        //    //        Price = d.Price
        //    //    }).ToList(),

        //    //    BonusInfo = user.BonusPoints < 100
        //    //    ? $"You currently have {user.BonusPoints} bonus points. Earn {100 - user.BonusPoints} more to unlock a bonus discount!"
        //    //    : $"You have {user.BonusPoints} bonus points. Great job!"
        //    //};
        //}

        public async Task<List<OrderReadDTO>> GetOrdersByUserAsync(string userId)
        {
            var orders = await _orderRepo.GetOrdersByUserAsync(userId);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var isPremium = await _userManager.IsInRoleAsync(user, "PremiumUser");

            return orders.Select(o =>
            {
                decimal originalTotal = o.Dishes.Sum(d => d.Price);
                decimal discount = originalTotal - o.TotalPrice;

                string bonusInfo = o.UsedBonus
                    ? $"You used 100 bonus points for a discount on this order. Current bonus: {user.BonusPoints}."
                    : isPremium
                        ? user.BonusPoints < 100
                            ? $"You currently have {user.BonusPoints} bonus points. Earn {100 - user.BonusPoints} more to unlock a bonus discount!"
                            : $"You have {user.BonusPoints} bonus points. Great job!"
                        : $"You are a RegularUser with {user.BonusPoints} bonus points.";

                return new OrderReadDTO
                {
                    Id = o.Id,
                    CreatedAt = o.CreatedAt,
                    OriginalTotalPrice = originalTotal,
                    DiscountApplied = discount,
                    TotalPrice = o.TotalPrice,
                    UsedBonus = o.UsedBonus,
                    Status = o.Status,
                    Dishes = o.Dishes.Select(d => new OrderDishDTO
                    {
                        Name = d.Name,
                        Price = d.Price
                    }).ToList(),
                    BonusInfo = bonusInfo
                };
            }).ToList();
        }

    }
}
