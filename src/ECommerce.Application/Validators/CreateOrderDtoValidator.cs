using ECommerce.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.BuyerId)
                .NotEmpty().WithMessage("Buyer ID is required");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Order must contain at least one item");

            RuleForEach(x => x.Items).SetValidator(new OrderItemRequestDtoValidator());
        }
    }
}
