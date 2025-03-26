namespace ShoppingService.Models
{
    /// <summary>
    /// DTO for transferring purchase information
    /// </summary>
    public class PurchaseRequest
    {
        public int ProductKey { get; set; }
        public int Quantity { get; set; }
    }
}