using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TomatoPizza.Core.Interfaces;
using TomatoPizza.Data.DTO.Orders;

namespace TomatoPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ✅ Helper method to extract user ID from token
        private string? GetUserId() =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // POST: api/Order/CreateOrder
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderCreateDTO dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _orderService.CreateOrderAsync(userId, dto);
            if (result == null)
                return BadRequest("Order could not be created.");

            return Ok(result);
        }

        // GET: api/Order/ViewMyOrder
        [HttpGet("ViewMyOrder")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var result = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(result);
        }
    }
}
