using MediatR;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var totalCount = products.Count;

        var items = products
            .OrderBy(product => product.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.Currency,
                product.CreatedAt,
                product.UpdatedAt))
            .ToList();

        return new PaginatedList<ProductDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}
