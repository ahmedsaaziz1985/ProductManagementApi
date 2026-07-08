using MediatR;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.DTOs;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _productRepository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            throw new ConflictException(
                nameof(request.Name),
                "A product with this name already exists.");
        }

        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.Currency);

        await _productRepository.AddAsync(product, cancellationToken);

        return MapToDto(product);
    }

    private static ProductDto MapToDto(Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.Currency,
            product.CreatedAt,
            product.UpdatedAt);
}
