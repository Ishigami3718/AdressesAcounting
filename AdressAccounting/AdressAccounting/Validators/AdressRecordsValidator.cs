using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Validators
{
    public class BaseAdressRecordsValidator:AbstractValidator<AdressRecord>
    {
        private readonly AdressAccountingContext _db;
        public BaseAdressRecordsValidator(AdressAccountingContext db)
        {
            _db = db;
            RuleFor(x => x.DateFrom)
                .NotEmpty().WithMessage("Дата початку не може бути порожньою")
                .LessThanOrEqualTo(x => x.DateTo)
                .WithMessage("Дата початку повинна бути раніше дати закінчення")
                .MustAsync(BeHigherThenLast)
                .WithMessage("Дата початку повинна бути пізніше останньої дати");
            RuleFor(x => x.DateTo)
                .NotEmpty().WithMessage("Дата закінчення не може бути порожньою")
                .GreaterThanOrEqualTo(x => x.DateFrom).WithMessage("Дата закінчення повинна być позже або рівна даті початку");
        }

        async Task<bool> BeHigherThenLast(DateOnly? date,CancellationToken cancellationToken)
        {
            bool exists = await _db.AdressRecords
                .AnyAsync(a => a.DateFrom > date, cancellationToken);
            return !exists;
        }
    }
}
