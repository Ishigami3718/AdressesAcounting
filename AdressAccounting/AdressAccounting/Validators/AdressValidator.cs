using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace AdressAccounting.Validators
{
    public class AdressValidator: AbstractValidator<Adress>
    {
        private readonly AdressService _service;
        public AdressValidator(AdressService service)
        {
            _service = service;
            RuleFor(a => a.StreetId).NotEmpty().WithMessage("Вулиця не може бути порожньою");
            RuleFor(a => a.Number)
                .GreaterThan(0).WithMessage("Номер цілий невід'ємний")
                .LessThanOrEqualTo(10000).WithMessage("Номер завеликий")
                .NotEmpty().WithMessage("Номер не може бути порожнім")
                .MustAsync((adress, number, cancellationToken) => 
                _service.BeUniqueNumberOnStreet(adress, number, cancellationToken))
                .WithMessage("Будинок з таким номером вже існує на цій вулиці.");
        }

    }
}
