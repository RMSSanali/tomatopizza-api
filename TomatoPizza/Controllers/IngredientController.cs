using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomatoPizza.Core.Interfaces;

namespace TomatoPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // GET: api/ingredient
        [HttpGet]
        [AllowAnonymous] // Optional: Allow open access to ingredient list
        public async Task<IActionResult> GetAll()
        {
            var result = await _ingredientService.GetAllAsync();
            return Ok(result);
        }
    }
}
