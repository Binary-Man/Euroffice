# Shopping Service

A simple shopping service implementation that allows you to add and remove products from a store with the option to purchase them.

## Overview

This project implements a shopping service following TDD (Test-Driven Development) approach. The system allows:
- Adding products to a shop inventory
- Removing products from inventory
- Purchasing products
- Tracking product quantities

## Project Structure

- **ShoppingService**: Core implementation of the shopping system
  - `IShoppingService.cs`: Interface defining core shopping operations
  - `ShoppingService.cs`: Implementation of the shopping service
  - `ProductStock.cs`: Internal class to manage product quantities
  - `Exceptions/`: Custom exception classes

- **ShoppingService.Models**: Data models
  - `Product.cs`: Model representing a product in the system 
  - `PurchaseRequest.cs`: DTO for purchase operations
  - `ProductWithStock.cs`: Product with stock information
  - `PurchaseResult.cs`: Result of purchase operations

- **ShoppingService.Tests**: Test suite for the shopping service
  - `ShoppingServiceTests.cs`: Tests for shopping service functionality
  - `ProductStockTests.cs`: Tests for the product stock class

## Key Features

- **Inventory Management**
  - Add new products to inventory
  - Update quantities of existing products
  - Remove products when stock is depleted

- **Purchase Processing**
  - Purchase products based on availability
  - Proper error handling for out-of-stock scenarios

- **Robust Error Handling**
  - Clear exception types for different error scenarios
  - Validation of all inputs

## How to Use

### Adding Products

```csharp
// Create a product
var product = new Product
{
    Key = 1,
    Sku = "ABC123",
    Name = "Example Product",
    Description = "This is a sample product",
    Category = "Electronics",
    Price = 99.99m
};

// Create shopping service
var shoppingService = new ShoppingService();

// Add product to inventory with quantity
shoppingService.AddProductToStock(product, 10);
```

### Checking Available Products

```csharp
// Get all products in stock
Product[] availableProducts = shoppingService.Products;

// Get quantity of a specific product
int stockQuantity = shoppingService.GetProduct(product);
```

### Purchasing Products

```csharp
try
{
    // Purchase a product
    shoppingService.PurchaseProduct(product, 2);
    Console.WriteLine("Purchase successful!");
}
catch (InsufficientStockException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
catch (ProductNotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Removing Products

```csharp
// Remove a product from inventory
shoppingService.RemoveProductFromStock(product, 3);
```

## Error Handling

The service provides specific exceptions for different error scenarios:

- `ProductNotFoundException`: When trying to access a product that doesn't exist
- `InsufficientStockException`: When trying to purchase more items than available
- `InvalidQuantityException`: When providing an invalid quantity (zero or negative)

Example of handling errors:

```csharp
try
{
    shoppingService.PurchaseProduct(product, quantity);
}
catch (ShoppingServiceException ex)
{
    // Handle all shopping service exceptions
    HandleError(ex.Message);
}
catch (Exception ex)
{
    // Handle unexpected errors
    LogError(ex);
}
```

## Testing

The project includes a comprehensive test suite built with xUnit. To run the tests:

1. Navigate to the ShoppingService.Tests directory
2. Run `dotnet test`

The tests cover:
- Basic functionality
- Edge cases
- Error conditions
- Boundary values

## Design Principles

This implementation follows S.O.L.I.D. principles:

- **Single Responsibility**: Classes have a single purpose
- **Open/Closed**: Design is open for extension but closed for modification
- **Liskov Substitution**: Implementations can be substituted for their interfaces
- **Interface Segregation**: Interface contains only what clients need
- **Dependency Inversion**: High-level modules depend on abstractions

## Requirements

- .NET 6.0 or later
- xUnit (for running tests)

