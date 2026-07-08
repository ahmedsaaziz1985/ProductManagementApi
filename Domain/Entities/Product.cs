using ProductManagementApi.Domain.Common;
using ProductManagementApi.Domain.Events;
using ProductManagementApi.Domain.Exceptions;
using ProductManagementApi.Domain.ValueObjects;

namespace ProductManagementApi.Domain.Entities;

public class Product : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public string Currency { get; private set; } = "USD";

    private Product()
    {
    }

    public static Product Create(string name, string description, decimal price, int stock, string currency = "USD")
    {
        ValidateName(name);
        ValidateStock(stock);
        var money = Money.Create(price, currency);

        var product = new Product
        {
            Name = name.Trim(),
            Description = description.Trim(),
            Price = money.Amount,
            Currency = money.Currency,
            Stock = stock
        };

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name));
        return product;
    }

    public void Update(string name, string description, decimal price, int stock, string currency = "USD")
    {
        ValidateName(name);
        ValidateStock(stock);
        var money = Money.Create(price, currency);

        Name = name.Trim();
        Description = description.Trim();
        Price = money.Amount;
        Currency = money.Currency;
        Stock = stock;
        MarkUpdated();
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be greater than zero.");
        }

        if (Stock < quantity)
        {
            throw new DomainException("Insufficient stock.");
        }

        Stock -= quantity;
        MarkUpdated();
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }
    }

    private static void ValidateStock(int stock)
    {
        if (stock < 0)
        {
            throw new DomainException("Stock cannot be negative.");
        }
    }
}
