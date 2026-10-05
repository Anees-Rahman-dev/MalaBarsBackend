using FluentValidation;
using FluentValidation.Validators;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Enter a valid Email address");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
