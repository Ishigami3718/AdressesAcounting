using AdressAccounting.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdressAccounting.UI
{
    public class MergeFrameViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Street> _streets;
        private ObservableCollection<Street> _selectedStreets = new();
        private bool _isHistorical;
        private string _name;
        private Street _selectedStreetResult;
        private Street _selectedStreetFromAll;
        private Street _selectedStreetFromMerging;

        public ObservableCollection<Street> Streets
        {
            get => _streets;
            set
            {
                _streets = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Street> SelectedStreets
        {
            get => _selectedStreets;
            set
            {
                _selectedStreets = value;
                OnPropertyChanged();
            }
        }

        public bool IsHistorical
        {
            get => _isHistorical;
            set
            {
                _isHistorical = value;
                if (IsHistorical) SelectedStreets = new();
                else
                {
                    Streets = new(_streetService.FilterByIsActual()
                    .Where(s => !s.SplitResults.Any() && !s.MergeRecords.Any()));
                    SelectedStreets = new();
                }
                OnPropertyChanged(nameof(IsHistorical));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }

        }

        public Street SelectedStreetResult
        {
            get => _selectedStreetResult;
            set
            {
                _selectedStreetResult = value;
                OnPropertyChanged(nameof(SelectedStreetResult));
            }
        }

        public Street SelectedStreetFromAll
        {
            get => _selectedStreetFromAll;
            set
            {
                _selectedStreetFromAll = value;
                OnPropertyChanged(nameof(SelectedStreetFromAll));
            }
        }

        public Street SelectedStreetFromMerging
        {
            get => _selectedStreetFromMerging;
            set
            {
                _selectedStreetFromMerging = value;
                OnPropertyChanged(nameof(SelectedStreetFromMerging));
            }
        }


        private readonly StreetService _streetService;
        private readonly StreetValidator _streetValidator;
        private readonly MergeService _mergeService;
        public MergeFrameViewModel(MergeService mergeService, StreetService service)
        {
            _streetService = service;
            _streetValidator = new();
            _mergeService = mergeService;
            Streets = new(_streetService.FilterByIsActual()
                    .Where(s => !s.SplitResults.Any() && !s.MergeRecords.Any()));
        }

        public void SwapStreetDirect(Street street)
        {
            SelectedStreets.Add(street);
            Streets.Remove(street);
        }

        public void SwapStreetReverse(Street street)
        {
            Streets.Add(street);
            SelectedStreets.Remove(street);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
