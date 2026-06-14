using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateOrderItemDto
    {
        [Required] public string ProductName { get; set; } = "";
        [Required][Range(1, 100)] public int Quantity { get; set; }
    }
}
