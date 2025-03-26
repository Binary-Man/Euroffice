namespace ShoppingService.Models
{
    /// <summary>
    /// DTO for transferring product with stock information
    /// </summary>
    public class ProductWithStock
    {
        public Product Product { get; set; }
        public int StockQuantity { get; set; }

        public ProductWithStock(Product product, int stockQuantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            StockQuantity = stockQuantity;
        }
    }
}