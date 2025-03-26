using ShoppingService.Exceptions;
using ShoppingService.Models;

namespace ShoppingService
{
    // Product Stock DTO to keep track of product quantities
    public class ProductStock
    {
        public Product Product { get; }
        public int Quantity { get; private set; }

        public ProductStock(Product? product, int quantity)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity to add must be positive", nameof(quantity));

            Quantity += quantity;
        }

        public void RemoveQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity to remove must be positive", nameof(quantity));

            if (quantity > Quantity)
                throw new InsufficientStockException($"Insufficient stock for product {Product.Name}. Available: {Quantity}, Requested: {quantity}");

            Quantity -= quantity;
        }
    }
}
