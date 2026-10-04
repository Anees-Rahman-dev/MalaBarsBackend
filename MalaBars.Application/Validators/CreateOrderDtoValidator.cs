using FluentValidation;
using FluentValidation.Validators;
using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.AddressId)
                .GreaterThan(0)
                .WithMessage("AddressId Must Be Greater Than 0");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment Method Is Required")
                .Must(BeValidPaymentMethod)
                .WithMessage("Payment Method Must Be COD, UPI, Or Card");                
        }

        private static bool BeValidPaymentMethod(string paymentMethod)
        {
            return paymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase)
                || paymentMethod.Equals("UPI", StringComparison.OrdinalIgnoreCase)
                || paymentMethod.Equals("Card", StringComparison.OrdinalIgnoreCase);
        }
    }
}
