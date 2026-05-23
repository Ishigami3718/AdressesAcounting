using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace AdressAccounting.Validators
{
    public class AdressValidator: AbstractValidator<Adress>
    {
        private readonly AdressAccountingContext _db;
        public AdressValidator(AdressAccountingContext db)
        {
            _db = db;
            RuleFor(a => a.Street).NotEmpty().WithMessage("Вулиця не може бути порожньою");
            RuleFor(a => a.Number)
                .GreaterThan(0).WithMessage("Номер цілий невід'ємний")
                .LessThanOrEqualTo(10000).WithMessage("Номер завеликий")
                .NotEmpty().WithMessage("Номер не може бути порожнім")
                .MustAsync(BeUniqueNumberOnStreet)
                .WithMessage("Будинок з таким номером вже існує на цій вулиці.");
        }

        private async Task<bool> BeUniqueNumberOnStreet(Adress address, int? number, CancellationToken cancellationToken)
        {
            bool exists = await _db.Adresses
                .AnyAsync(a => a.StreetId == address.StreetId
                            && a.Number == number
                            && a.Id != address.Id,
                          cancellationToken);
            return !exists;
        }
    }
}
