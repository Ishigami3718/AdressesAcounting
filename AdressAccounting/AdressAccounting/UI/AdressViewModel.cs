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
        private DateOnly? _selectedDateFromFilter;
        private DateOnly? _selectedDateToFilter;
        private Adress _selectedAdress;
        private string _countOfAdresses;
        public bool IsAdressSelected => SelectedAdress != null;

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

        public DateOnly? SelectedDateFromFilter
        {
            get => _selectedDateFromFilter;
            set
            {
                _selectedDateFromFilter = value;
                LoadAdresses();
                OnPropertyChanged(nameof(SelectedDateFromFilter));
            }
        }

        public DateOnly? SelectedDateToFilter
        {
            get => _selectedDateToFilter;
            set
            {
                _selectedDateToFilter = value;
                LoadAdresses();
                OnPropertyChanged(nameof(SelectedDateToFilter));
            }
        }

        public Adress SelectedAdress
        {
            get => _selectedAdress;
            set
            {
                _selectedAdress = value;
                OnPropertyChanged(nameof(SelectedAdress));
                OnPropertyChanged(nameof(IsAdressSelected));
            }
        }

        public string CountOfAdresses
        {
            get => $"Кількість адрес: {_countOfAdresses}";
            set
            {
                _countOfAdresses = value;
                OnPropertyChanged(nameof(CountOfAdresses));
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
            var adresses = _adressService.GetFilteredAdresses(IsActualFilter, 
                IsHasHistoryFilter, SelectedAdressFilter, SelectedStreetFilter, 
                SelectedDateFromFilter, SelectedDateToFilter);
            CountOfAdresses = adresses.Count().ToString();

            Adresses = new ObservableCollection<Adress>(adresses);
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

        public void DeleteAdress(Adress adress)
        {
            _adressService.DeleteAdress(adress);
            LoadAdresses();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
