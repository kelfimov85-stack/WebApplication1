using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly DeliveryDbContext _db;

        public OrdersController(DeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _db.Orders
                .Include(order => order.Customer)
                .Include(order => order.Items)
                .Include(order => order.Courier).ToList();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _db.Orders
                .Include(order => order.Customer)
                .Include(order => order.Items)
                .Include(order => order.Courier)
                .FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto dto)
        {
            var isCustomerExists = _db.Customers.Any(customer => customer.Id == dto.CustomerId);

            if (!isCustomerExists)
                return NotFound("Customer not found");

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                Status = OrderStatus.Created,
                Items = dto.Items.Select(item => new OrderItem()
                {
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                }).ToList()
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            return Created("", order);
        }

        [HttpPatch("{id}/assign-courier")]
        public IActionResult AssignCourier(int id, AssignCourierDto dto)
        {
            var order = _db.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound("Order not found");

            if (order.CourierId != null)
            {
                return BadRequest("Courier already assigned");
            }

            var courier = _db.Couriers.FirstOrDefault(courier => courier.Id == dto.CourierId);

            if (courier == null)
                return NotFound("Courier not found");

            order.CourierId = dto.CourierId;
            order.Status = OrderStatus.Assigned;

            _db.SaveChanges();

            return Ok(order);
        }

        [HttpPatch("{id}/deliver")]
        public IActionResult DeliverOrder(int id)
        {
            var order = _db.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound("Order not found");

            if (order.CourierId == null)
            {
                return BadRequest("Courier not assigned");
            }

            order.Status = OrderStatus.Delivered;
            _db.SaveChanges();

            return Ok(order);
        }

        [HttpPatch("{id}/cancel")]
        public IActionResult DeliverOrderCancelled(int id) 
        {
            var status = _db.Orders.Any(order => order.Id == id);

            if (status)
            {
                var order = _db.Orders.FirstOrDefault(order => order.Id == id);
                order.Status = OrderStatus.Cancelled;
                return Ok(order);
            }

            return BadRequest();
        }

        [HttpGet("Assigned")]
        public IActionResult GetOrders([FromQuery] OrderStatus? status)
        {
            var query = _db.Orders
                .Include(o => o.Items)
                .AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }       

            var orders = query
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
