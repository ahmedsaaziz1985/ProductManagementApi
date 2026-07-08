using MediatR;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.DTOs;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (await _productRepository.ExistsByNameExceptIdAsync(request.Name, request.Id, cancellationToken))
        {
            throw new ConflictException(
                nameof(request.Name),
                "A product with this name already exists.");
        }

        product.Update(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.Currency);

        await _productRepository.UpdateAsync(product, cancellationToken);

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
