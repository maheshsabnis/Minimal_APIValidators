namespace Minimal_APIValidators.Models
{
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }

    }
}
