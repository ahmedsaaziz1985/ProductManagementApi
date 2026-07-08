using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.Description)
            .HasMaxLength(1000);

        builder.Property(product => product.Price)
            .HasPrecision(18, 2);

        builder.Property(product => product.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        builder.HasIndex(product => product.Name)
            .IsUnique();
    }
}
