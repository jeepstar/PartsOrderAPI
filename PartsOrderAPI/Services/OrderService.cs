using PartsOrderAPI.Models;
using PartsOrderAPI.DTO;
using PartsOrderAPI.Data;

namespace PartsOrderAPI.Services
{
    public class OrderService : IOrderService
    {
        public Order CreateOrder(OrderDto orderDto)
        {
            var lineItems = new List<LineItem>();

            foreach (var item in orderDto.LineItems)
            {
                var part = InMemoryDataStore.Parts.FirstOrDefault(p => p.Id == item.PartId);
                if (part == null)
                {
                    throw new KeyNotFoundException($"Part with ID {item.PartId} not found.");
                }

                lineItems.Add(new LineItem
                {
                    PartId = item.PartId,
                    Quantity = item.Quantity,
                    Part = part
                });
            }

            var order = new Order
            {
                OrderNumber = InMemoryDataStore.Orders.Count + 1,
                LineItems = lineItems
            };

            InMemoryDataStore.Orders.Add(order);
            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            return InMemoryDataStore.Orders;
        }
    }
}
