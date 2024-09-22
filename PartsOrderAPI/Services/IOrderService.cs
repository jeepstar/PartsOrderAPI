using PartsOrderAPI.DTO;
using PartsOrderAPI.Models;
namespace PartsOrderAPI.Services
{
    public interface IOrderService
    {
        Order CreateOrder(OrderDto orderDto);
        IEnumerable<Order> GetOrders();
    }
}
