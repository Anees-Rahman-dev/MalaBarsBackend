using FluentValidation;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Image is required");

            RuleFor(x => x.Stock)
                .NotEmpty()
                .WithMessage("Stock cannot be negative");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating must be between 0 and 5");
        }
    }
}
