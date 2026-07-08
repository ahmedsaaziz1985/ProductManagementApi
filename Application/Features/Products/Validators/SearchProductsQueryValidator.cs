using FluentValidation;
using ProductManagementApi.Application.Features.Products.Queries.SearchProducts;

namespace ProductManagementApi.Application.Features.Products.Validators;

public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsQueryValidator()
    {
        RuleFor(query => query.Search)
            .NotEmpty()
            .WithMessage("Search parameter is required.");

        RuleFor(query => query.PageNumber)
            .GreaterThan(0);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100);
    }
}
