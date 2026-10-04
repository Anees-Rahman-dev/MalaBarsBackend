using FluentValidation;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name Is Required")
                .MinimumLength(2)
                .WithMessage("Name at least be 2 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email Must Be Entered")
                .EmailAddress()
                .WithMessage("Enter a valid Email address");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password Is Required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters");
        }
    }
}
