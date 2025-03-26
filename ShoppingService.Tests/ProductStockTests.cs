using ShoppingService.Exceptions;
using ShoppingService.Models;

// Additional test class for ProductStock
namespace ShoppingService.Tests
{
    public class ProductStockTests

    {
        private readonly Product _testProduct;


        public ProductStockTests()
        {
            _testProduct = new Product
            {
                Key = 1,
                Sku = "ABC123",
                Name = "Test Product"
            };

        }

        [Fact]
        public void Constructor_SetsProperties_WhenValidArguments()
        {
            // Arrange
            const int quantity = 10;

            // Act
            var productStock = new ProductStock(_testProduct, quantity);

            // Assert
            Assert.Equal(_testProduct, productStock.Product);
            Assert.Equal(quantity, productStock.Quantity);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenProductIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductStock(null, 10));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenQuantityIsNegative()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ProductStock(_testProduct, -5));
        }

        [Fact]
        public void AddQuantity_IncreasesQuantity_WhenQuantityIsPositive()
        {
            // Arrange
            const int initialQuantity = 5;
            const int addedQuantity = 7;
            var productStock = new ProductStock(_testProduct, initialQuantity);

            // Act
            productStock.AddQuantity(addedQuantity);

            // Assert
            Assert.Equal(initialQuantity + addedQuantity, productStock.Quantity);
        }

        [Fact]
        public void AddQuantity_ThrowsException_WhenQuantityIsZero()
        {
            // Arrange
            var productStock = new ProductStock(_testProduct, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productStock.AddQuantity(0));
        }

        [Fact]
        public void AddQuantity_ThrowsException_WhenQuantityIsNegative()
        {
            // Arrange
            var productStock = new ProductStock(_testProduct, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productStock.AddQuantity(-3));
        }

        [Fact]
        public void RemoveQuantity_DecreasesQuantity_WhenQuantityIsValid()
        {
            // Arrange
            const int initialQuantity = 10;
            const int removedQuantity = 4;
            var productStock = new ProductStock(_testProduct, initialQuantity);

            // Act
            productStock.RemoveQuantity(removedQuantity);

            // Assert
            Assert.Equal(initialQuantity - removedQuantity, productStock.Quantity);
        }

        [Fact]
        public void RemoveQuantity_ThrowsException_WhenQuantityExceedsAvailable()
        {
            // Arrange
            const int initialQuantity = 5;
            const int removedQuantity = 10;
            var productStock = new ProductStock(_testProduct, initialQuantity);

            // Act & Assert
            Assert.Throws<InsufficientStockException>(() =>
                productStock.RemoveQuantity(removedQuantity));
        }

        [Fact]
        public void RemoveQuantity_ThrowsException_WhenQuantityIsZero()
        {
            // Arrange
            var productStock = new ProductStock(_testProduct, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productStock.RemoveQuantity(0));
        }

        [Fact]
        public void RemoveQuantity_ThrowsException_WhenQuantityIsNegative()
        {
            // Arrange
            var productStock = new ProductStock(_testProduct, 5);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => productStock.RemoveQuantity(-2));
        }
    }
}