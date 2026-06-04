using AdressAccounting.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdressAccounting.UI
{
    public class StreetCRWindowViewModel : INotifyPropertyChanged
    {

        private readonly StreetValidator _validator;
        private readonly StreetNameRecordsValidator _streetNameRecordsValidator;
        private readonly StreetService _streetService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        private string _streetName;
        private bool _isActual;
        private Street _selectedRelatedStreet;
        private DateTime? _dateFromCreating;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;

        public ObservableCollection<Street> Streets { get; set; }

        public string StreetName
        {
            get => _streetName;
            set
            {
                if (_streetName != value)
                {
                    _streetName = value;
                    OnPropertyChanged(nameof(StreetName));
                }
            }
        }

        public bool IsActual
        {
            get => _isActual;
            set
            {
                if (_isActual != value)
                {
                    _isActual = value;
                    OnPropertyChanged(nameof(IsActual));
                }
            }
        }

        public Street SelectedRelatedStreet
        {
            get => _selectedRelatedStreet;
            set
            {
                if (_selectedRelatedStreet != value)
                {
                    _selectedRelatedStreet = value;
                    OnPropertyChanged(nameof(SelectedRelatedStreet));
                }
            }
        }

        public DateTime? DateFromCreating
        {
            get => _dateFromCreating;
            set
            {
                    _dateFromCreating = value;
                    OnPropertyChanged(nameof(DateFromCreating));
            }
        }
        public DateTime? DateFrom
        {
            get => _dateFrom;
            set
            {
                
                    _dateFrom = value;
                    OnPropertyChanged(nameof(DateFrom));
                
            }
        }

        public DateTime? DateTo
        {
            get => _dateTo;
            set
            {
                _dateTo = value;
                OnPropertyChanged(nameof(DateTo));

            }
        }

        private bool _isDateFromCreatingValid;

        public bool IsDateFromCreatingValid
        {
            get => _isDateFromCreatingValid;
            set
            {
                _isDateFromCreatingValid = value;
                OnPropertyChanged(nameof(IsDateFromCreatingValid));
            }
        }

        public StreetCRWindowViewModel(StreetService streetService, StreetNameHistoryService streetNameHistoryService)
        {

            _streetService = streetService;
            _streetNameHistoryService = streetNameHistoryService;
            _validator = new StreetValidator();
            _streetNameRecordsValidator = new StreetNameRecordsValidator();
            Streets = new ObservableCollection<Street>(_streetService.FilterByIsActual());
            IsActual = true;
        }

        private int _idRedact;
        public StreetCRWindowViewModel(StreetService streetService, Street street)
        {
            _streetService = streetService;
            Streets = new ObservableCollection<Street>(_streetService.FilterByIsActual());
            StreetName = street.Name;
            IsActual = street.IsActual;
            _idRedact = street.Id;
            _validator = new StreetValidator();
        }



        public bool CreateRedactStreet(bool isRedact = false)
        {
            bool result = false;
            if (IsActual)
            {
                Street street = new Street
                {
                    Name = StreetName,
                    IsActual = true,
                    Id = _idRedact
                };
                var validationResult = _validator.Validate(street);
                if (validationResult.IsValid)
                {

                    if (!isRedact)
                        _streetService.CreateStreet(StreetName, DateFromCreating);
                    else
                        _streetService.RedactStreet(street);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                StreetNameRecord streetNameRecord = new StreetNameRecord
                {
                    Name = StreetName,
                    DateFrom = DateFrom.HasValue ? DateOnly.FromDateTime(DateFrom.Value) : (DateOnly?)null,
                    DateTo = DateTo.HasValue ? DateOnly.FromDateTime(DateTo.Value) : (DateOnly?)null
                };
                StreetNameRecordsStreet streetNameRecordsStreet = new StreetNameRecordsStreet
                {
                    StreetId = SelectedRelatedStreet.Id,
                    StreetNameRecords = streetNameRecord
                };
                var validationResult = _streetNameRecordsValidator.Validate(streetNameRecordsStreet.StreetNameRecords);
                if (validationResult.IsValid)
                {
                    _streetNameHistoryService.AddStreetNameRecordStreet(streetNameRecordsStreet);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
