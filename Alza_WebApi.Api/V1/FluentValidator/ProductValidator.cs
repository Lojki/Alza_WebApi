using Alza_WebApi.Api.V1.ResponseModel;
using FluentValidation;
using System;

namespace Alza_WebApi.Api.V1.FluentValidator
{
    /// <summary>
    /// Response model Product fluent validator
    /// </summary>
    public class ProductValidator : AbstractValidator<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductValidator"/> class.
        /// </summary>
        public ProductValidator()
        {
            RuleFor(p => p.Id).NotNull().GreaterThan(0);

            RuleFor(p => p.Name).NotEmpty().WithMessage("The {PropertyName} is required.");

            RuleFor(p => p.ImgUri).NotEmpty().WithMessage("The {PropertyName} is required.")
                .Must(uri => Uri.TryCreate(uri?.OriginalString, UriKind.Absolute, out _)).WithName("Image URI").WithMessage("The {PropertyName} is not valid URI.");

            RuleFor(p => p.Price).NotNull().WithMessage("The {PropertyName} is required.")
                .GreaterThan(0).WithMessage("The {PropertyName} must be positive number.");

            RuleFor(p => p.Available).NotNull().WithMessage("The {PropertyName} is required.");
        }
    }
}
