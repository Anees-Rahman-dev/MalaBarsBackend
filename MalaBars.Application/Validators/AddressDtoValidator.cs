using FluentValidation;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MinimumLength(2)
                .WithMessage("Full Name must be at least 2 characters");

            RuleFor(x => x.Phone)
               .NotEmpty()
               .WithMessage("Phone number is required.")
               .Matches(@"^[0-9]{10}$")
               .WithMessage("Phone number must be exactly 10 digits.");

            RuleFor(x => x.AddressLine)
               .NotEmpty()
               .WithMessage("Address line is required");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required");

            RuleFor(x => x.Pincode)
                .NotEmpty()
                .WithMessage("Pincode is required.")
                .Matches(@"^[0-9]{6}$")
                .WithMessage("Pincode must be exactly 6 digits.");
        }
    }
}
