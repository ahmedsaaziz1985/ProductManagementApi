namespace ProductManagementApi.Application.Features.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string Currency,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string Currency = "USD");

public record UpdateProductDto(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string Currency = "USD");
