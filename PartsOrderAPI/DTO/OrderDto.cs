using System.ComponentModel.DataAnnotations;

namespace PartsOrderAPI.DTO
{
    public class OrderDto
    {
        [Required(ErrorMessage = "LineItems are required.")]
        public List<LineItemDto> LineItems { get; set; }
    }
}
