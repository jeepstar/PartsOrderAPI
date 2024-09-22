namespace PartsOrderAPI.Models
{
    public class LineItem
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Quantity * Part.Price;
        public Part Part { get; set; }
    }
}
