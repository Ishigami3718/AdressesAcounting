using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Validators
{
    public class StreetNameRecordsValidator:AbstractValidator<StreetNameRecord>
    {
        public StreetNameRecordsValidator()
        {
            RuleFor(s => s.Name).NotEmpty().WithMessage("Назва не може бути порожньою");
            RuleFor(s => s.DateTo).NotEmpty().WithMessage("Дата не може бути порожньою")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Дата закінчення не може бути раніше поточної дати")
                .GreaterThanOrEqualTo(s => s.DateFrom)
                .WithMessage("Дата закінчення не може бути пізніше поточної дати");
            RuleFor(s => s.DateFrom).NotEmpty().WithMessage("Дата не може бути порожньою")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Дата початку не може бути пізніше поточної дати")
                .LessThanOrEqualTo(s => s.DateTo)
                .WithMessage("Дата початку не може бути пізніше дати закінчення");
        }
    }
}
