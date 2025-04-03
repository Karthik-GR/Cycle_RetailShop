using backend.Models;

namespace backend.DTO
{
    public class CartItemDto
    {
        public int ProductId { get; set; } // Updated from InventoryId
        public int Quantity { get; set; }
    }

    public class OrderPlacementDto
    {
        public long CustomerPhone { get; set; }
    }

    public class OrderStatusUpdateDto
    {
        public OrderStatus Status { get; set; }
    }

    public class InvoiceDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class InvoiceItemDto
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
