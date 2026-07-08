using MediatR;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.Application.Features.Products.Queries.SearchProducts;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public SearchProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PaginatedList<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.SearchAsync(
            request.Search,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var items = products
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
