
namespace ShoppingService.Exceptions
{
// Domain Exceptions
    public class ShoppingServiceException : Exception
    {
        public ShoppingServiceException(string message) : base(message) { }
    }

    public class ProductNotFoundException : ShoppingServiceException
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    public class InsufficientStockException : ShoppingServiceException
    {
        public InsufficientStockException(string message) : base(message) { }
    }

    public class InvalidQuantityException : ShoppingServiceException
     {
        public InvalidQuantityException(string message) : base(message) { }
    }
}