using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; } = 0;
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public int OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
    }
}
