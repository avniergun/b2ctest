namespace B2C.WepApi.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string? Barcode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
