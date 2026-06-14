using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateOrderItemDto
    {
        [Required] public string ProductName { get; set; } = "";
        [Required][Range(1, 100)] public int Quantity { get; set; }
        [Required][Range(0.01, 1000000)] public decimal Price { get; set; }
    }
}
