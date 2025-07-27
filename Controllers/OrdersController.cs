using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order and applies applicable discounts.
        /// </summary>
        /// <param name="request">Order details</param>
        /// <returns>Created order</returns>
        
        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] OrderRequest request)
        {
            var customer = new Customer { Id = request.CustomerId, Segment = request.Segment };
            var order = _orderService.CreateOrder(customer, request.TotalAmount);
            return Ok(order);
        }

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="status">New status</param>
        /// <returns>Success or failure</returns>
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(Guid id, [FromQuery] OrderStatus status)
        {
            bool updated = _orderService.UpdateStatus(id, status);
            return updated ? Ok() : BadRequest("Invalid transition");
        }

        /// <summary>
        /// Retrieves analytics for orders (e.g., average order value, fulfillment time).
        /// </summary>
        /// <returns>Analytics data</returns>
        [HttpGet("analytics")]
        public IActionResult GetAnalytics()
        {
            var (avg, time) = _orderService.GetAnalytics();
            return Ok(new { avgOrderValue = avg, avgFulfillmentTimeMinutes = time });
        }
    }

    public class OrderRequest
    {
        public Guid CustomerId { get; set; }
        public CustomerSegment Segment { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
