namespace ProductManagementApi.Domain.Events;

public sealed class ProductCreatedEvent(Guid productId, string name) : IDomainEvent
{
    public Guid ProductId { get; } = productId;
    public string Name { get; } = name;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
