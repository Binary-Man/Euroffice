namespace ShoppingService.Models
{
    /// <summary>
    /// DTO for purchase result information
    /// </summary>
    public class PurchaseResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public static PurchaseResult Successful(Product product, int quantity)
        {
            return new PurchaseResult
            {
                Success = true,
                Message = "Purchase successful",
                Product = product,
                Quantity = quantity,
                TotalPrice = product.Price * quantity
            };
        }

        public static PurchaseResult Failed(string errorMessage)
        {
            return new PurchaseResult
            {
                Success = false,
                Message = errorMessage
            };
        }
    }
}