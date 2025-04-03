using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Product
    {
        [Key] public int Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Brand { get; set; }

        [Required] public string Category { get; set; }

        public string? Type { get; set; } // Road, Mountain, Hybrid, Electric

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] ImageFile { get; set; }
    }
}
