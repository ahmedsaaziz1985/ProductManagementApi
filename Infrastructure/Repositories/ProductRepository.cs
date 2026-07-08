using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Domain.Entities;
using ProductManagementApi.Infrastructure.Persistence;

namespace ProductManagementApi.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context)
    : BaseRepository<Product>(context), IProductRepository
{
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .AnyAsync(product => product.Name == name.Trim(), cancellationToken);
    }

    public async Task<bool> ExistsByNameExceptIdAsync(string name, Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .AnyAsync(product => product.Name == name.Trim() && product.Id != id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> SearchAsync(
        string search,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var term = search.Trim();
        var currencyTerm = term.ToUpper();

        decimal? priceFilter = decimal.TryParse(term, out var price) ? price : null;
        int? stockFilter = int.TryParse(term, out var stock) ? stock : null;

        var query = Context.Products.AsNoTracking().Where(product =>
            product.Name.ToLower().Contains(term.ToLower()) ||
            product.Currency == currencyTerm ||
            (priceFilter.HasValue && product.Price == priceFilter.Value) ||
            (stockFilter.HasValue && product.Stock == stockFilter.Value));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(product => product.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
