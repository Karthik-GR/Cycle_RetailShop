namespace backend.DTO
{
    public class Inventory_dto
    {
        public string? CycleName { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        //public DateTime? AddedDate { get; set; }
        //public String? image_url { get; set; }
        public byte[]? ImageFile { get; set; }
    }

    public class InventoryCreateDto
    {
        public string CycleName { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile? ImageFile { get; set; }
        //public List<string>? Colors { get; set; } 
    }


    public class InventoryUpdateDto
    {
        public string? CycleName { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
