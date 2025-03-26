using ShoppingService.Exceptions;
using ShoppingService.Interfaces;
using ShoppingService.Models;

namespace ShoppingService
{

    // Implementation of IShoppingService interface
    public class ShoppingService : IShoppingService
    {
        private readonly Dictionary<int, ProductStock> _inventory;

        public ShoppingService()
        {
            _inventory = new Dictionary<int, ProductStock>();
        }

        public Product[] Products => _inventory.Values.Select(ps => ps.Product).ToArray();

        public void AddProductToStock(Product? product, int qty)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (qty <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            if (_inventory.TryGetValue(product.Key, out ProductStock? existingStock))
            {
                existingStock.AddQuantity(qty);
            }
            else
            {
                _inventory[product.Key] = new ProductStock(product, qty);
            }
        }

        public void RemoveProductFromStock(Product? product, int qty)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (qty <= 0)
                throw new InvalidQuantityException("Quantity must be greater than zero");

            if (!_inventory.TryGetValue(product.Key, out ProductStock? productStock))
            {
                throw new ProductNotFoundException($"Product with ID {product.Key} not found in inventory");
            }

            productStock.RemoveQuantity(qty);

            // Remove the product from inventory if stock becomes zero
            if (productStock.Quantity == 0)
            {
                _inventory.Remove(product.Key);
            }
        }

        public void PurchaseProduct(Product product, int qty)
        {
            RemoveProductFromStock(product, qty);
        }

        // Additional helper methods not in the interface
        public int GetProduct(Product? product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (_inventory.TryGetValue(product.Key, out ProductStock? productStock))
            {
                return productStock.Quantity;
            }

            return 0;
        }
    }
}
