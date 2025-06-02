using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Dishes;

namespace TomatoPizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // GET: api/dish
        [HttpGet ("ViewAllDishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dishService.GetAllDishesAsync();
            return Ok(dishes);
        }

        // GET: api/dish/{id}
        [HttpGet("ViewDishByID{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
                return NotFound();

            return Ok(dish);
        }

        // POST: api/dish
        [Authorize(Roles = "Admin")]
        [HttpPost ("CreateDishByAdmin")]
        public async Task<IActionResult> CreateDish([FromBody] DishCreateDTO dto)
        {
            var result = await _dishService.CreateDishAsync(dto);
            if (!result)
                return BadRequest("Could not create dish.");

            return Ok("Dish created successfully.");
        }

        // PUT: api/dish/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDish{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] DishUpdateDTO dto)
        {
            var result = await _dishService.UpdateDishAsync(id, dto);
            if (!result)
                return NotFound("Dish not found.");

            return Ok("Dish updated successfully.");
        }

        // DELETE: api/dish/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDish{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var result = await _dishService.DeleteDishAsync(id);
            if (!result)
                return NotFound("Dish not found.");

            return Ok("Dish deleted successfully.");
        }
    }
}
