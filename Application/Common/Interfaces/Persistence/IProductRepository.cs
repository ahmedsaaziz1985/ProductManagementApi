using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.Application.Common.Interfaces.Persistence;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameExceptIdAsync(string name, Guid id, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Product> Items, int TotalCount)> SearchAsync(
        string search,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
