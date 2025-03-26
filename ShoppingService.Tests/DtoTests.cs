using ShoppingService.Models;

namespace ShoppingService.Tests
{
    public class DtoTests
    {
        #region ProductWithStock Tests

        [Fact]
        public void ProductWithStock_Constructor_SetsProperties()
        {
            // Arrange
            var product = new Product
            {
                Key = 1,
                Sku = "ABC123",
                Name = "Test Product",
                Description = "Test Description",
                Category = "Test Category",
                Price = 9.99m
            };
            var stockQuantity = 10;

            // Act
            var productWithStock = new ProductWithStock(product, stockQuantity);

            // Assert
            Assert.Equal(product, productWithStock.Product);
            Assert.Equal(stockQuantity, productWithStock.StockQuantity);
        }

        [Fact]
        public void ProductWithStock_Constructor_ThrowsException_WhenProductIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductWithStock(null, 10));
        }

        [Fact]
        public void ProductWithStock_Usage_InProductListing()
        {
            // Arrange
            var service = new ShoppingService();
            var product1 = new Product { Key = 1, Sku = "SKU001", Name = "Product 1", Price = 10.99m };
            var product2 = new Product { Key = 2, Sku = "SKU002", Name = "Product 2", Price = 19.99m };
            
            service.AddProductToStock(product1, 5);
            service.AddProductToStock(product2, 3);
            
            // Act - This simulates how ProductWithStock would be used in an application
            var productsWithStock = new ProductWithStock[service.Products.Length];
            for (int i = 0; i < service.Products.Length; i++)
            {
                var product = service.Products[i];
                var stock = service.GetProduct(product);
                productsWithStock[i] = new ProductWithStock(product, stock);
            }
            
            // Assert
            Assert.Equal(2, productsWithStock.Length);
            Assert.Equal(product1, productsWithStock[0].Product);
            Assert.Equal(5, productsWithStock[0].StockQuantity);
            Assert.Equal(product2, productsWithStock[1].Product);
            Assert.Equal(3, productsWithStock[1].StockQuantity);
        }

        #endregion

        #region PurchaseRequest Tests

        [Fact]
        public void PurchaseRequest_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var purchaseRequest = new PurchaseRequest
            {
                ProductKey = 42,
                Quantity = 3
            };

            // Assert
            Assert.Equal(42, purchaseRequest.ProductKey);
            Assert.Equal(3, purchaseRequest.Quantity);
        }

        [Fact]
        public void PurchaseRequest_Usage_InPurchaseOperation()
        {
            // Arrange
            var service = new ShoppingService();
            var product = new Product { Key = 1, Sku = "SKU001", Name = "Test Product", Price = 10.99m };
            service.AddProductToStock(product, 10);
            
            var purchaseRequest = new PurchaseRequest
            {
                ProductKey = product.Key,
                Quantity = 3
            };
            
            // Act - This simulates how PurchaseRequest would be processed
            Product? productToPurchase = null;
            foreach (var p in service.Products)
            {
                if (p.Key == purchaseRequest.ProductKey)
                {
                    productToPurchase = p;
                    break;
                }
            }
            
            service.PurchaseProduct(productToPurchase, purchaseRequest.Quantity);
            
            // Assert
            Assert.Equal(7, service.GetProduct(product)); // 10 - 3 = 7
        }

        [Fact]
        public void PurchaseRequest_Usage_WithMultipleRequests()
        {
            // Arrange
            var service = new ShoppingService();
            var product1 = new Product { Key = 1, Sku = "SKU001", Name = "Product 1", Price = 10.99m };
            var product2 = new Product { Key = 2, Sku = "SKU002", Name = "Product 2", Price = 19.99m };
            
            service.AddProductToStock(product1, 10);
            service.AddProductToStock(product2, 5);
            
            var purchaseRequests = new[]
            {
                new PurchaseRequest { ProductKey = 1, Quantity = 2 },
                new PurchaseRequest { ProductKey = 2, Quantity = 1 }
            };
            
            // Act - Process multiple purchase requests
            foreach (var request in purchaseRequests)
            {
                Product? productToPurchase = null;
                foreach (var p in service.Products)
                {
                    if (p.Key == request.ProductKey)
                    {
                        productToPurchase = p;
                        break;
                    }
                }
                
                if (productToPurchase != null)
                {
                    service.PurchaseProduct(productToPurchase, request.Quantity);
                }
            }
            
            // Assert
            Assert.Equal(8, service.GetProduct(product1)); // 10 - 2 = 8
            Assert.Equal(4, service.GetProduct(product2)); // 5 - 1 = 4
        }

        #endregion

        #region PurchaseResult Tests

        [Fact]
        public void PurchaseResult_Successful_CreatesSuccessfulResult()
        {
            // Arrange
            var product = new Product
            {
                Key = 1,
                Sku = "ABC123",
                Name = "Test Product",
                Price = 9.99m
            };
            int quantity = 3;

            // Act
            var result = PurchaseResult.Successful(product, quantity);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Purchase successful", result.Message);
            Assert.Equal(product, result.Product);
            Assert.Equal(quantity, result.Quantity);
            Assert.Equal(product.Price * quantity, result.TotalPrice);
        }

        [Fact]
        public void PurchaseResult_Failed_CreatesFailedResult()
        {
            // Arrange
            string errorMessage = "Product not found";

            // Act
            var result = PurchaseResult.Failed(errorMessage);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(errorMessage, result.Message);
            Assert.Null(result.Product);
            Assert.Equal(0, result.Quantity);
            Assert.Equal(0, result.TotalPrice);
        }

        [Fact]
        public void PurchaseResult_Usage_InPurchaseOperation()
        {
            // Arrange
            var service = new ShoppingService();
            var product = new Product { Key = 1, Sku = "SKU001", Name = "Test Product", Price = 10.99m };
            service.AddProductToStock(product, 5);
            
            // Act - Simulate purchase operation with result
            PurchaseResult result;
            try
            {
                service.PurchaseProduct(product, 3);
                result = PurchaseResult.Successful(product, 3);
            }
            catch (Exception ex)
            {
                result = PurchaseResult.Failed(ex.Message);
            }
            
            // Assert
            Assert.True(result.Success);
            Assert.Equal(product, result.Product);
            Assert.Equal(3, result.Quantity);
            Assert.Equal(product.Price * 3, result.TotalPrice);
        }

        [Fact]
        public void PurchaseResult_Usage_InFailedPurchaseOperation()
        {
            // Arrange
            var service = new ShoppingService();
            var product = new Product { Key = 1, Sku = "SKU001", Name = "Test Product", Price = 10.99m };
            service.AddProductToStock(product, 2);
            
            // Act - Simulate purchase operation with insufficient stock
            PurchaseResult result;
            try
            {
                service.PurchaseProduct(product, 5); // More than available
                result = PurchaseResult.Successful(product, 5);
            }
            catch (Exception ex)
            {
                result = PurchaseResult.Failed(ex.Message);
            }
            
            // Assert
            Assert.False(result.Success);
            Assert.Contains("Insufficient stock", result.Message);
            Assert.Null(result.Product);
            Assert.Equal(0, result.Quantity);
            Assert.Equal(0, result.TotalPrice);
        }

        #endregion
    }
}