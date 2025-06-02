using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TomatoPizza.Data.Entities;
using TomatoPizza.Data.Identity;

namespace TomatoPizza
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ✅ Entity sets (tables)
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Order> Orders { get; set; }

        // OPTIONAL: Fluent API if you need advanced many-to-many config
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Optional: If you want to configure the join table name
            builder.Entity<Dish>()
                .HasMany(d => d.Orders)
                .WithMany(o => o.Dishes)
                .UsingEntity(j => j.ToTable("DishOrders"));

            builder.Entity<Dish>()
                .HasMany(d => d.Ingredients)
                .WithMany(i => i.Dishes)
                .UsingEntity(j => j.ToTable("DishIngredients"));
        }
    }
}
