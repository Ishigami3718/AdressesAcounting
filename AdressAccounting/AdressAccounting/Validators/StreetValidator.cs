using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Validators
{
    public class StreetValidator: AbstractValidator<Street>
    {
        public StreetValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ім'я вулиці не може бути порожнім")
                .MaximumLength(100).WithMessage("Ім'я вулиці не може перевищувати 100 символів.");
        }
    }
}
