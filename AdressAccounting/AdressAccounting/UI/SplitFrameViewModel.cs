using AdressAccounting.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdressAccounting.UI
{
    public class SplitFrameViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Street> _streets;
        private ObservableCollection<Street> _selectedStreets = new();
        private bool _isHistorical;
        private string _name;
        private Street _selectedStreetResult;
        private Street _selectedStreetFromAll;
        private Street _selectedStreetFromMerging;
        private bool _isAutomaticRenumeration;
        private List<(Street, int)> newStreetsNumbers;
        private string _newName;

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
                UpdateStreets();
                OnPropertyChanged(nameof(IsHistorical));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateStreets();
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

        public bool IsAutomaticRenumeration
        {
            get => _isAutomaticRenumeration;
            set
            {
                _isAutomaticRenumeration = value;
                OnPropertyChanged(nameof(IsAutomaticRenumeration));
            }
        }

        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }


        private readonly StreetService _streetService;
        private readonly StreetValidator _streetValidator;
        private readonly SplitService _splitService;
        public SplitFrameViewModel(SplitService splitService, StreetService service)
        {
            _streetService = service;
            _streetValidator = new();
            _splitService = splitService;
            UpdateStreets();
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

        private void UpdateStreets()
        {
            Streets = new(_streetService.FilterByIsActual()
                    .Where(s => !s.SplitResults.Any() && !s.MergeRecords.Any()).OrderBy(s => s.Name));
            if (!string.IsNullOrEmpty(Name)) Streets =
                    new ObservableCollection<Street>(Streets.Where(s => s.Name.ToLower().Contains(Name.ToLower())));
        }

        public void GetNewNumbers()
        {
            try
            {
                AdressRenumeringWindow window = new AdressRenumeringWindow(SelectedStreetResult, SelectedStreets);
                window.ShowDialog();
                newStreetsNumbers = Enumerable.Range(0, SelectedStreets.Count)
                    .Select(i => (window.NewNumbers_[i].Street, window.NewNumbers_[i].Number)).ToList();
            }
            catch (Exception ex) { }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
