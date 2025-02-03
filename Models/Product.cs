namespace ProductModule.Models
{
    public class Product
    {
        public string? ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? PK { get; set;}
        public string? SK { get; set;}
        public required string ProductCategoryId { get; set; }
    }
}