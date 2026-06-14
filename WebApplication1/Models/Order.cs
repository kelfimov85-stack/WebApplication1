namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? CourierId { get; set; }
        public Courier? Courier { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem>? Items { get; set; }
    }
}
