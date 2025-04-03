namespace backend.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string Username { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; } // Changed from InventoryId
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; } // Updated to Product
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; } // Changed from InventoryId
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; } // Updated to Product
    }
}
