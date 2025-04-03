using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            var username = User.Identity.Name;
            var cartItems = await _context.CartItems
                                          .Include(ci => ci.Product) // Updated to Product
                                          .Where(ci => ci.Cart.Username == username)
                                          .ToListAsync();
            return Ok(cartItems);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            var username = User.Identity.Name;
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Username == username);
            if (cart == null)
            {
                cart = new Cart
                {
                    Username = username,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = new CartItem
            {
                ProductId = cartItemDto.ProductId, // Changed from InventoryId
                Quantity = cartItemDto.Quantity,
                CartId = cart.CartId
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item added to cart", cartItem });
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderPlacementDto orderDto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.phoneNumber == orderDto.CustomerPhone);
            if (customer == null)
            {
                return BadRequest(new { error = "Invalid customer phone number" });
            }

            var username = User.Identity.Name;
            var cart = await _context.Carts.Include(c => c.CartItems)
                                           .FirstOrDefaultAsync(c => c.Username == username);
            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest(new { error = "Cart is empty" });
            }

            var order = new Order
            {
                CustomerId = customer.customerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _context.Products.FindAsync(cartItem.ProductId); // Changed from Inventory
                if (product == null) continue;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                });
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order placed successfully", order });
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { error = "User is not authenticated" });
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.email == username);
            if (customer == null)
            {
                return BadRequest(new { error = "Customer not found" });
            }

            var orders = await _context.Orders
                                .Include(o => o.OrderItems)
                                .Where(o => o.CustomerId == customer.customerId)
                                .ToListAsync();

            return Ok(orders);
        }


        [HttpPatch("orders/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto statusDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound(new { error = "Order not found" });
            }

            order.Status = statusDto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order status updated successfully", order });
        }

        [HttpGet("orders/{orderId}/invoice")]
        public async Task<IActionResult> GetInvoice(int orderId)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Product)
                                      .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound(new { error = "Order not found" });
            }

            var invoice = new InvoiceDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerName = (await _context.Customers.FindAsync(order.CustomerId))?.name,
                Items = order.OrderItems.Select(oi => new InvoiceItemDto
                {
                    ProductName = oi.Product.Name, // Updated field
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.UnitPrice * oi.Quantity
                }).ToList(),
                SubTotal = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                Tax = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity) * 0.1M,
                GrandTotal = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Quantity) * 1.1M
            };

            return Ok(invoice);
        }
    }
}
