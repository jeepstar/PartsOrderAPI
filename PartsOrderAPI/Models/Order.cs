namespace PartsOrderAPI.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public List<LineItem> LineItems { get; set; }
        public decimal TotalCost => LineItems.Sum(item => item.Total);
    }
}
