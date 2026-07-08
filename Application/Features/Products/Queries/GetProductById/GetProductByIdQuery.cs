using MediatR;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
