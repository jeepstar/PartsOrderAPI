using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartsOrderAPI.DTO;
using PartsOrderAPI.Services;
using Serilog;
using System.Text.Json;

namespace PartsOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly Serilog.ILogger _logger;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
            _logger = Log.ForContext<OrdersController>();
        }

       

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDto">Order data transfer object</param>
        /// <returns>Created order</returns>
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDto orderDto)
        {
            _logger.Information("CreateOrder method called.");

            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for CreateOrder.");
                return BadRequest(ModelState);
            }

            try
            {
                var order = _orderService.CreateOrder(orderDto);
                _logger.Information("Order created successfully with OrderNumber: {OrderNumber}", order.OrderNumber);
                return CreatedAtAction(nameof(GetOrders), new { id = order.OrderNumber }, order);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while creating order. {ex.Message}");
                
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets the list of orders.
        /// </summary>
        /// <returns>List of orders</returns>
        [HttpGet]
        public IActionResult GetOrders()
        {
            _logger.Information("GetOrders method called.");

            try
            {
                var orders = _orderService.GetOrders();
                _logger.Information("Orders retrieved successfully.");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the orders.");
            }
        }
    }
}
