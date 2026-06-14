using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreateOrderDto
    {
        [Required] public int CustomerId { get; set; }
        [Required] public List<CreateOrderItemDto> Items { get; set; }
    }
}
