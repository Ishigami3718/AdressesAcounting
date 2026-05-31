using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Validators
{
    public class AdressRecordsValidator:AbstractValidator<AdressRecord>
    {
        public AdressRecordsValidator()
        {
            RuleFor(x => x.DateFrom)
                .NotEmpty().WithMessage("Дата початку не може бути порожньою")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                /*.LessThanOrEqualTo(x => x.DateTo)
                .WithMessage("Дата початку повинна бути раніше дати закінчення")
                /*.MustAsync(BeHigherThenLast)
                .WithMessage("Дата початку повинна бути пізніше останньої дати")*/;
            /*RuleFor(x => x.DateTo)
                .NotEmpty().WithMessage("Дата закінчення не може бути порожньою")
                .GreaterThanOrEqualTo(x => x.DateFrom)
                .WithMessage("Дата закінчення повинна бути після дати початку")
                /*.MustAsync(BeLowerThenLast)
                .WithMessage("Дата закінчення повинна бути раніше поточної дати")*/;
        }

        /*async Task<bool> BeHigherThenLast(DateOnly? date,CancellationToken cancellationToken)
        {
            bool exists = await _db.AdressRecords
                .AnyAsync(a => a.DateFrom > date, cancellationToken);
            return !exists;
        }

        async Task<bool> BeLowerThenLast(DateOnly? date, CancellationToken cancellationToken)
        {
            bool exists = await _db.AdressRecords
                .AnyAsync(a => a.DateTo < DateOnly.FromDateTime(DateTime.Now), cancellationToken);
            return !exists;
        }*/
    }
}
