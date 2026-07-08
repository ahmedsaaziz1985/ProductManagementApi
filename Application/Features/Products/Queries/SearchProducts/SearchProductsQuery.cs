using MediatR;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.Application.Features.Products.Queries.SearchProducts;

public record SearchProductsQuery(
    string Search,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PaginatedList<ProductDto>>;
