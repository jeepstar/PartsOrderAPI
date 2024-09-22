using PartsOrderAPI.Attributes;
using System.ComponentModel.DataAnnotations;
namespace PartsOrderAPI.DTO
{
    public class LineItemDto
    {
        [Required (ErrorMessage = "PartId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PartId must be a positive integer.")]
        [ValidInteger]
        public int PartId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        [ValidInteger]
        public int Quantity { get; set; }
    }
}
