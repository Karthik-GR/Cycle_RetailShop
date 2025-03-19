using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CycleName { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Type { get; set; } // Road, Mountain, Hybrid, Electric

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        //[Required]
        //public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        //public string image_url { get; set; }
        [Required]
        public byte[] ImageFile { get; set; }

        //public string Colors { get; set; }

    }

    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int BrandId { get; set; }
        [Required]public string BrandName { get; set; }  
    }

    public class CycleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int CycleTypeId { get; set; }
        [Required] public string CycleTypeName { get; set; }

    }
}
