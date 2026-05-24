using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdressAccounting.UI
{
    public class AdressViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Adress> _adresses;
        public ObservableCollection<Street> Streets { get; set; }
        private readonly AdressService _adressService;
        private readonly StreetService _streetService;
        private bool _isActualFilter;
        private bool _isHasHistoryFilter;
        private string _selectedAdressFilter;
        private Street _selectedStreetFilter;

        public ObservableCollection<Adress> Adresses
        {
            get { return _adresses; }
            set
            {
                _adresses = value;
                OnPropertyChanged(nameof(Adresses));
            }
        }

        public bool IsActualFilter
        {
            get => _isActualFilter;
            set
            {
                _isActualFilter = value;
                LoadAdresses();
                OnPropertyChanged(nameof(IsActualFilter));
            }
        }

        public bool IsHasHistoryFilter
        {
            get => _isHasHistoryFilter;
            set
            {
                _isHasHistoryFilter = value;
                LoadAdresses();
                OnPropertyChanged(nameof(IsHasHistoryFilter));
            }
        }

        public string SelectedAdressFilter
        {
            get => _selectedAdressFilter;
            set
            {
                _selectedAdressFilter = value;
                LoadAdresses();
                Debug.WriteLine($"SelectedAdressFilter set to: {_selectedAdressFilter}");
                OnPropertyChanged(nameof(SelectedAdressFilter));
            }
        }

        public Street SelectedStreetFilter
        {
            get => _selectedStreetFilter;
            set
            {
                _selectedStreetFilter = value;
                LoadAdresses();
                OnPropertyChanged(nameof(SelectedStreetFilter));
            }
        }
        public AdressViewModel(AdressService adressService, StreetService streetService)
        {
            _adressService = adressService;
            _streetService = streetService;
            LoadStreets();
            LoadAdresses();
        }

        private void LoadAdresses()
        {
            var adresses = _adressService.GetAllAdresses();
            //checkbox filters
            if (IsActualFilter)
            {
                adresses = _adressService.GetActualAdresses();
            }
            if (IsHasHistoryFilter)
            {
                adresses = _adressService.GetAdressesWithHistory();
            }
            if (!string.IsNullOrEmpty(SelectedAdressFilter))
            {
                if(int.TryParse(SelectedAdressFilter, out int number))
                {
                    adresses = _adressService.GetAdressByNumber(number);
                }
                else throw new ArgumentException("SelectedAdressFilter must be a valid number.");
            }
            if (SelectedStreetFilter != null)
            {
                adresses = _adressService.GetAdressesByStreet(SelectedStreetFilter);
            }

            Adresses = new ObservableCollection<Adress>(adresses.ToList());
        }

        private void LoadStreets()
        {
            var streets = _streetService.GetAllStreets();
            Streets = new ObservableCollection<Street>(streets);
        }

        public void AddAdress(Adress adress)
        {
            _adressService.CreateAdress(adress);
            LoadAdresses();
        }

        public void UpdateAdress(Adress adress, int newNumber)
        {
            _adressService.UpdateAdress(adress, newNumber);
            LoadAdresses();
        }

        public void RedactAdress(Adress adress)
        {
            _adressService.RedactAdress(adress);
            LoadAdresses();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
