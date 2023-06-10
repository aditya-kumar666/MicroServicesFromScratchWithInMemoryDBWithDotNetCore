using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        IOrdersProvider ordersProvider;
        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var orders = await ordersProvider.GetOrdersAsync(customerId);
            if (orders.IsSuccess)
            {
                return Ok(orders.Orders);
            }
            return NotFound();
        }
    }
}
