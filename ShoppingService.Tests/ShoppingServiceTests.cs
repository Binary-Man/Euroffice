
using ShoppingService.Exceptions;
using ShoppingService.Models;
using System;
using System.Linq;
using Xunit;

namespace ShoppingService.Tests
{
    public class ShoppingServiceTests
    {
        private readonly ShoppingService _service;
        private readonly Product _testProduct;
        //private readonly Product _emptyProduct;

        public ShoppingServiceTests()
        {
            _service = new ShoppingService();
            _testProduct = new Product
            {
                Key = 1,
                Sku = "ABC123",
                Name = "Test Product",
                Description = "A test product",
                Category = "Test",
                Price = 9.99m
            };
            //_emptyProduct = new Product();
        }

        [Fact]
        public void Products_ReturnsEmptyArray_WhenNoProductsAdded()
        {
            // Act
            var products = _service.Products;

            // Assert
            Assert.Empty(products);
        }

        [Fact]
        public void AddProductToStock_AddsNewProduct_WhenProductDoesNotExist()
        {
            // Arrange
            const int quantity = 10;

            // Act
            _service.AddProductToStock(_testProduct, quantity);

            // Assert
            Assert.Single(_service.Products);
            Assert.Equal(_testProduct, _service.Products[0]);
            Assert.Equal(quantity, _service.GetProduct(_testProduct));
        }

        [Fact]
        public void AddProductToStock_IncreasesQuantity_WhenProductExists()
        {
            // Arrange
            const int initialQuantity = 5;
            const int additionalQuantity = 7;
            _service.AddProductToStock(_testProduct, initialQuantity);

            // Act
            _service.AddProductToStock(_testProduct, additionalQuantity);

            // Assert
            Assert.Single(_service.Products);
            Assert.Equal(initialQuantity + additionalQuantity, _service.GetProduct(_testProduct));
        }

        [Fact]
        public void AddProductToStock_ThrowsException_WhenProductIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.AddProductToStock(null, 10));
        }

        [Fact]
        public void AddProductToStock_ThrowsException_WhenQuantityIsZero()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidQuantityException>(() => 
                _service.AddProductToStock(_testProduct, 0));
            Assert.Contains("greater than zero", exception.Message);
        }

        [Fact]
        public void AddProductToStock_ThrowsException_WhenQuantityIsNegative()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidQuantityException>(() => 
                _service.AddProductToStock(_testProduct, -5));
            Assert.Contains("greater than zero", exception.Message);
        }

        [Fact]
        public void RemoveProductFromStock_DecreasesQuantity_WhenProductExists()
        {
            // Arrange
            const int initialQuantity = 10;
            const int quantityToRemove = 3;
            _service.AddProductToStock(_testProduct, initialQuantity);

            // Act
            _service.RemoveProductFromStock(_testProduct, quantityToRemove);

            // Assert
            Assert.Equal(initialQuantity - quantityToRemove, _service.GetProduct(_testProduct));
        }

        [Fact]
        public void RemoveProductFromStock_RemovesProduct_WhenQuantityBecomesZero()
        {
            // Arrange
            const int quantity = 5;
            _service.AddProductToStock(_testProduct, quantity);

            // Act
            _service.RemoveProductFromStock(_testProduct, quantity);

            // Assert
            Assert.Empty(_service.Products);
            Assert.Equal(0, _service.GetProduct(_testProduct));
        }

        [Fact]
        public void RemoveProductFromStock_ThrowsException_WhenProductDoesNotExist()
        {
            // Act & Assert
            var exception = Assert.Throws<ProductNotFoundException>(() => 
                _service.RemoveProductFromStock(_testProduct, 1));
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void RemoveProductFromStock_ThrowsException_WhenQuantityGreaterThanAvailable()
        {
            // Arrange
            const int availableQuantity = 3;
            const int requestedQuantity = 5;
            _service.AddProductToStock(_testProduct, availableQuantity);

            // Act & Assert
            var exception = Assert.Throws<InsufficientStockException>(() => 
                _service.RemoveProductFromStock(_testProduct, requestedQuantity));
            Assert.Contains("Insufficient stock", exception.Message);
        }

        [Fact]
        public void RemoveProductFromStock_ThrowsException_WhenProductIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.RemoveProductFromStock(null, 1));
        }

        [Fact]
        public void RemoveProductFromStock_ThrowsException_WhenQuantityIsZero()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidQuantityException>(() => 
                _service.RemoveProductFromStock(_testProduct, 0));
            Assert.Contains("greater than zero", exception.Message);
        }

        [Fact]
        public void RemoveProductFromStock_ThrowsException_WhenQuantityIsNegative()
        {
            // Act & Assert
            var exception = Assert.Throws<InvalidQuantityException>(() => 
                _service.RemoveProductFromStock(_testProduct, -1));
            Assert.Contains("greater than zero", exception.Message);
        }

        [Fact]
        public void PurchaseProduct_CallsRemoveProductFromStock()
        {
            // Arrange
            const int initialQuantity = 10;
            const int purchaseQuantity = 3;
            _service.AddProductToStock(_testProduct, initialQuantity);

            // Act
            _service.PurchaseProduct(_testProduct, purchaseQuantity);

            // Assert
            Assert.Equal(initialQuantity - purchaseQuantity, _service.GetProduct(_testProduct));
        }

        [Fact]
        public void GetProductStock_ReturnsZero_WhenProductDoesNotExist()
        {
            // Act
            var stock = _service.GetProduct(_testProduct);

            // Assert
            Assert.Equal(0, stock);
        }

        [Fact]
        public void GetProductStock_ReturnsCorrectQuantity_WhenProductExists()
        {
            // Arrange
            const int quantity = 15;
            _service.AddProductToStock(_testProduct, quantity);

            // Act
            var stock = _service.GetProduct(_testProduct);

            // Assert
            Assert.Equal(quantity, stock);
        }

        [Fact]
        public void GetProductStock_ThrowsException_WhenProductIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.GetProduct(null));
        }

        [Fact]
        public void Products_ReturnsAllProducts_WhenMultipleProductsExist()
        {
            // Arrange
            var product1 = new Product { Key = 1, Name = "Product 1", Sku = "SKU001" };
            var product2 = new Product { Key = 2, Name = "Product 2", Sku = "SKU002" };
            var product3 = new Product { Key = 3, Name = "Product 3", Sku = "SKU003" };
            
            _service.AddProductToStock(product1, 10);
            _service.AddProductToStock(product2, 5);
            _service.AddProductToStock(product3, 15);

            // Act
            var products = _service.Products;

            // Assert
            Assert.Equal(3, products.Length);
            Assert.Contains(product1, products);
            Assert.Contains(product2, products);
            Assert.Contains(product3, products);
        }
    }
   
}