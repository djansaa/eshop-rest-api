namespace eshop_rest_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUri { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}
