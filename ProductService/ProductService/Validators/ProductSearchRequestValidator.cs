using FluentValidation;

public class ProductSearchRequestValidator : AbstractValidator<ProductSearchRequest>
{
    public ProductSearchRequestValidator()
    {
        // Paging
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        // Discount range
        RuleFor(x => x)
            .Must(x => !x.DiscountFrom.HasValue || !x.DiscountTo.HasValue || x.DiscountFrom <= x.DiscountTo)
            .WithMessage("DiscountFrom must be less than or equal to DiscountTo.");

        // Price range
        RuleFor(x => x)
            .Must(x => !x.PriceFrom.HasValue || !x.PriceTo.HasValue || x.PriceFrom <= x.PriceTo)
            .WithMessage("PriceFrom must be less than or equal to PriceTo.");
    }
}
