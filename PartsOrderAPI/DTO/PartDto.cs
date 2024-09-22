using PartsOrderAPI.Attributes;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace PartsOrderAPI.DTO
{
    public class PartDto
    {
        [Required(ErrorMessage = "Description is required.")]
        [ValidPattern(@"^[a-zA-Z0-9\s!@#$%^&*()\-_=+{}[\]|\\:;""'<>,.?/]{1,200}$", ErrorMessage = "Description must be between 1 and 200 alphanumeric characters and can include common special characters.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [ValidDecimal]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        [ValidInteger]
        public int Quantity { get; set; }
    }
}
