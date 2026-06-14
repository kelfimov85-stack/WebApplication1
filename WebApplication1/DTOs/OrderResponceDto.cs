using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; } 
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Cost => Price * Quantity; 
    }

}
