using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.DTO
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; } // e.g., "Cycle" or "Accessory"
        public string? Type { get; set; } // Additional type info
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }

    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
