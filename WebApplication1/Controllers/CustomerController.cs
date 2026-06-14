using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly DeliveryDbContext _db;

        public CustomerController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet("{customerId}/orders")]
        public IActionResult GetCustomerOrders(int customerId) 
        {
            var customerExists = _db.Customers
                .Any(c => c.Id == customerId);

            if (!customerExists)
            {
                return NotFound(new { message = $"Клиент с ID {customerId} не найден." });
            }

            var orders = _db.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        ProductName = i.ProductName,
                        Price = i.Price,
                        Quantity = i.Quantity
                    }).ToList(),

                    TotalPrice = o.Items.Sum(i => i.Price * i.Quantity)
                })
                .ToList();

            return Ok(orders);
        }
    }
}
