using FluentValidation;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class UpdateCartQuantityDtoValidator : AbstractValidator<UpdateCartQuantityDto>
    {
        public UpdateCartQuantityDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");
        }
    }
}
