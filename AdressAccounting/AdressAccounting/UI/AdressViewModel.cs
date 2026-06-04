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
        private bool _isHasRelatedAdressOnSameArea;
        private string _selectedAdressFilter;
        private Street _selectedStreetFilter;
        private DateTime? _selectedDateFromFilter;
        private DateTime? _selectedDateToFilter;
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
                _ = LoadAdresses();
                OnPropertyChanged(nameof(IsActualFilter));
            }
        }

        public bool IsHasHistoryFilter
        {
            get => _isHasHistoryFilter;
            set
            {
                _isHasHistoryFilter = value;
                _ = LoadAdresses();
                OnPropertyChanged(nameof(IsHasHistoryFilter));
            }
        }

        public bool IsHasRelatedAdressOnSameArea
        {
            get => _isHasRelatedAdressOnSameArea;
            set
            {
                _isHasRelatedAdressOnSameArea = value;
                _ = LoadAdresses();
                OnPropertyChanged(nameof(IsHasRelatedAdressOnSameArea));
            }
        }

        public string SelectedAdressFilter
        {
            get => _selectedAdressFilter;
            set
            {
                _selectedAdressFilter = value;
                _ = LoadAdresses();
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
                _ = LoadAdresses();
                OnPropertyChanged(nameof(SelectedStreetFilter));
            }
        }

        public DateTime? SelectedDateFromFilter
        {
            get => _selectedDateFromFilter;
            set
            {
                _selectedDateFromFilter = value;
                _ = LoadAdresses();
                OnPropertyChanged(nameof(SelectedDateFromFilter));
            }
        }

        public DateTime? SelectedDateToFilter
        {
            get => _selectedDateToFilter;
            set
            {
                _selectedDateToFilter = value;
                _ = LoadAdresses();
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
            _ = LoadAdresses();
        }

        private async Task LoadAdresses()
        {
            var adresses = await _adressService.GetFilteredAdresses(IsActualFilter, 
                IsHasHistoryFilter, IsHasRelatedAdressOnSameArea, SelectedAdressFilter, SelectedStreetFilter, 
                SelectedDateFromFilter.HasValue 
                ? DateOnly.FromDateTime((DateTime)SelectedDateFromFilter) : (DateOnly?)null, 
                SelectedDateToFilter.HasValue 
                ? DateOnly.FromDateTime((DateTime)SelectedDateToFilter) : (DateOnly?)null);
            CountOfAdresses = adresses.Count().ToString();

            Adresses = new ObservableCollection<Adress>(adresses);
        }

        private void LoadStreets()
        {
            var streets = _streetService.GetAllStreets().ToList();
            streets.Insert(0, new Street { Name = "Всі вулиці", Id = 0 });
            Streets = new ObservableCollection<Street>(streets);
        }

        public async Task AddAdress()
        {
            new AdressCRWindow(_adressService, _streetService).ShowDialog();
            await LoadAdresses();
        }

        public async Task AddAdressOnSameArea()
        {
            new AdressCRWindow(_adressService, _streetService, SelectedAdress).ShowDialog();
            await LoadAdresses();
        }

        public async Task UpdateAdress()
        {
            //update window
            new AdressCRWindow(_adressService,_streetService, SelectedAdress,true).ShowDialog();
            
            await LoadAdresses();
        }

        public async Task RedactAdress(Adress adress)
        {
            _adressService.RedactAdress(adress);
            await LoadAdresses();
        }

        public async Task DeleteAdress(Adress adress)
        {
            _adressService.DeleteAdress(adress);
            await LoadAdresses();
        }

        public async Task AddHistory(Adress adress)
        {
            new AdressCRWindow(_adressService, _streetService, adress, isHistoryMode: true).ShowDialog();
            await LoadAdresses();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
