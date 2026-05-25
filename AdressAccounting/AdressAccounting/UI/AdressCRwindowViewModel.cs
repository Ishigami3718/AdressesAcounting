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

        private DateOnly? _selectedDateFrom;
        public DateOnly? SelectedDateFrom
        {
            get => _selectedDateFrom;
            set
            {
                _selectedDateFrom = value;
                IsDateFromValid = _selectedDateFrom.HasValue;
                OnPropertyChanged(nameof(SelectedDateFrom));
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

        public async Task<bool> CreateAdress()
        {
            ResetValidation();
            Adress newAdress = new Adress
            {
                Street = SelectedStreet ?? null,
                StreetId = SelectedStreet?.Id ?? 0,
                IsActual = IsActual,
                AdressRecords = new List<AdressRecord> { new AdressRecord
                { DateFrom = SelectedDateFrom } },
                Number = string.IsNullOrEmpty(this.Number) ? 0 : int.TryParse(this.Number, out var number) ? number : 0
            };
            var validationResult = await _validator.ValidateAsync(newAdress);
            if (validationResult.IsValid && IsDateFromValid)
            {
                _adressService.CreateAdress(newAdress);
                return true;
            }
            else
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
                return false;
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
