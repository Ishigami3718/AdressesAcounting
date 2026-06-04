using AdressAccounting.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AdressAccounting.UI
{
    public class AdressCRwindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Street> Streets { get; set; }
        public bool IsActual { get; set; }

        private DateTime? _selectedDateFrom;
        private DateTime? _selectedDateTo;
        public DateTime? SelectedDateFrom
        {
            get => _selectedDateFrom;
            set
            {
                _selectedDateFrom = value;
                IsDateFromValid = _selectedDateFrom.HasValue && _selectedDateFrom.Value <= DateTime.Now;
                OnPropertyChanged(nameof(SelectedDateFrom));
            }
        }

        public DateTime? SelectedDateTo
        {
            get => _selectedDateTo;
            set
            {
                _selectedDateTo = value;
                OnPropertyChanged(nameof(SelectedDateTo));
            }
        }

        public string Number { get; set; }
        private Street _selectedStreet;
        public Street SelectedStreet
        {
            get => _selectedStreet;
            set
            {
                _selectedStreet = value;
                IsStreetValid = value != null;
                OnPropertyChanged(nameof(SelectedStreet));
            }
        }

        private bool _isStreetValid = true;
        private string _streetValidationMessage;
        private bool _isNumberValid = true;
        private string _numberValidationMessage;
        private bool _isDateFromValid = true;
        public bool IsDateFromValid
        {
            get => _isDateFromValid;
            set { _isDateFromValid = value; OnPropertyChanged(nameof(IsDateFromValid)); }
        }
        public bool IsStreetValid
        {
            get => _isStreetValid;
            set
            {
                _isStreetValid = value;
                OnPropertyChanged(nameof(IsStreetValid));
            }
        }

        public string NumberValidationMessage
        {
            get => _numberValidationMessage;
            set
            {
                _numberValidationMessage = value;
                OnPropertyChanged(nameof(NumberValidationMessage));
            }
        }

        public bool IsNumberValid
        {
            get => _isNumberValid;
            set
            {
                _isNumberValid = value;
                OnPropertyChanged(nameof(IsNumberValid));
            }
        }

        public string StreetValidationMessage
        {
            get => _streetValidationMessage;
            set
            {
                _streetValidationMessage = value;
                OnPropertyChanged(nameof(StreetValidationMessage));
            }
        }

        private readonly AdressValidator _validator;
        private readonly AdressService _adressService;
        private readonly StreetService _streetService;

        public AdressCRwindowViewModel(AdressService service, StreetService streetService)
        {
            _adressService = service;
            _streetService = streetService;
            _validator = new(_adressService);
            Streets = new ObservableCollection<Street>(_streetService.GetAllStreets());
        }

        private int _areaId;
        private bool _isHistoryMode;
        private AdressRecordsValidator _adressRecordValidator;
        public AdressCRwindowViewModel(AdressService service, StreetService streetService, Adress adress,
            bool isHistoryMode = false)
        {
            _adressService = service;
            _streetService = streetService;
            _validator = new(_adressService);
            Streets = new ObservableCollection<Street>(_streetService.GetAllStreets());
            if (isHistoryMode)
            {
                IsActual = false;
                _isHistoryMode = isHistoryMode;
                _adressRecordValidator = new();
            }
            else
            {
                SelectedStreet = adress.Street;
                Number = adress.Number.ToString();
                _areaId = adress.AreaId.Value;
            }
        }

        public async Task<bool> CreateAdress()
        {
            ResetValidation();
            IsDateFromValid = SelectedDateFrom.HasValue;
            Adress newAdress = new Adress
            {
                Street = SelectedStreet ?? null,
                StreetId = SelectedStreet?.Id ?? 0,
                IsActual = IsActual,
                AdressRecords = new List<AdressRecord> { new AdressRecord
                { DateFrom = SelectedDateFrom.HasValue ?
                DateOnly.FromDateTime((DateTime)SelectedDateFrom) : (DateOnly?)null, 
                    StreetName = SelectedStreet.Name } },
                Number = string.IsNullOrEmpty(this.Number) ? 0 : 
                int.TryParse(this.Number, out var number) ? number : 0
                
            };
            if (_areaId != 0)
            {
                newAdress.AreaId = _areaId;
                newAdress.IsActual = false;
            }
            else 
            {
                newAdress.Area = new AreaBuilding();
                foreach (var record in newAdress.AdressRecords)
                {
                    record.Area = newAdress.Area;
                }
            }
            var validationResult = await _validator.ValidateAsync(newAdress);
            if (validationResult.IsValid && IsDateFromValid)
            {
                _adressService.CreateAdress(newAdress);
                return true;
            }
            else
            {
                ShowValidation(validationResult);
                return false;
            }
        }

        public async Task<bool> UpdateAdress(Adress adress)
        {
            //Update validation
            ResetValidation();
            if (!int.TryParse(this.Number, out int parsedNumber))
            {
                parsedNumber = 0;
            }
            Adress adressToValidate = new Adress
            {
                Id = adress.Id,
                StreetId = SelectedStreet?.Id ?? 0,
                IsActual = IsActual,
                Number = parsedNumber
            };
            var validationResult = await _validator.ValidateAsync(adressToValidate);
            if (validationResult.IsValid)
            {
                _adressService.UpdateAdress(adress, parsedNumber, SelectedStreet);
                return true;
            }
            else
            {
                ShowValidation(validationResult);
                return false;
            }
        }

        public async Task<bool> AddHistory(Adress adress)
        {
            ResetValidation();
            if (!SelectedDateFrom.HasValue)
            {
                IsDateFromValid = false;
                return false;
            }
            if (!int.TryParse(this.Number, out int parsedNumber))
            {
                parsedNumber = 0;
            }
            AdressRecord adressRecordToValidate = new AdressRecord
            {
                AdressId = adress.Id,
                DateFrom = DateOnly.FromDateTime((DateTime)SelectedDateFrom),
                DateTo = DateOnly.FromDateTime((DateTime)SelectedDateTo),
                Number = parsedNumber,
                StreetName = SelectedStreet.Name
            };
            bool isDateRangeValid =  SelectedDateFrom.Value <= SelectedDateTo.Value && 
                SelectedDateTo.Value <= DateTime.Today 
                && DateOnly.FromDateTime(SelectedDateTo.Value) < adress.AdressRecords.Max(r => r.DateFrom);
            var validationResult = _adressRecordValidator.Validate(adressRecordToValidate);
            if (validationResult.IsValid && isDateRangeValid)
            {
                _adressService.AddAdressRecord(adressRecordToValidate);
                return true;
            }
            else
            {
                ShowValidation(validationResult);
                return false;
            }
        }

        private void ShowValidation(FluentValidation.Results.ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                switch (error.PropertyName)
                {
                    case nameof(Adress.Number):
                        IsNumberValid = false;
                        NumberValidationMessage = error.ErrorMessage;
                        break;
                    case nameof(Adress.Street):
                        IsStreetValid = false;
                        StreetValidationMessage = error.ErrorMessage;
                        break;
                }

            }
        }
        private void ResetValidation()
        {
            IsStreetValid = true;
            StreetValidationMessage = string.Empty;
            IsNumberValid = true;
            NumberValidationMessage = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
