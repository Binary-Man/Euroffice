using ShoppingService.Models;

namespace ShoppingService.Interfaces
{
    public interface IShoppingService
    {
        Product[] Products { get; }

        void AddProductToStock(Product product, int qty);        
        void RemoveProductFromStock(Product product, int qty);
        void PurchaseProduct(Product product, int qty);
        int GetProduct(Product product);
    }
}
