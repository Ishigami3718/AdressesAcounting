using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace AdressAccounting.UI
{
    public class StreetFrameViewModel : INotifyPropertyChanged
    {
    
        private ObservableCollection<Street> _streets;
        private Street _selectedStreet;
        private bool _isHasMergeParentsFilter;
        private bool _isHasSplitParentFilter;
        private bool _isHasHistoryFilter;
        private bool _isSortedByNameFilter;
        private DateTime? _selectedDateFromFilter;
        private DateTime? _selectedDateToFilter;
        private string _nameSearchStreetFilter;
        private string _countOfStreets;
        private bool _isActualFilter;

        public string NameSearchStreetFilter
        {
            get { return _nameSearchStreetFilter; }
            set
            {
                _nameSearchStreetFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(NameSearchStreetFilter));
            }
        }

        public bool IsHasMergeParentsFilter
        {
            get { return _isHasMergeParentsFilter; }
            set
            {
                _isHasMergeParentsFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(IsHasMergeParentsFilter));
            }
        }

        public bool IsHasSplitParentFilter
        {
            get { return _isHasSplitParentFilter; }
            set
            {
                _isHasSplitParentFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(IsHasSplitParentFilter));
            }
        }

        public bool IsHasHistoryFilter
        {
            get { return _isHasHistoryFilter; }
            set
            {
                _isHasHistoryFilter = value;
                if (!_isActualFilter)
                {
                    SelectedDateFromFilter = null;
                    SelectedDateToFilter = null;
                }
                LoadStreets();
                OnPropertyChanged(nameof(IsHasHistoryFilter));
            }
        }

        public bool IsSortedByNameFilter
        {
            get { return _isSortedByNameFilter; }
            set
            {
                _isSortedByNameFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(IsSortedByNameFilter));
            }
        }

        public bool IsStreetSelected => SelectedStreet != null;

        public string CountOfStreets
        {
            get { return $"Кількість адрес: {_countOfStreets}"; }
            set
            {
                _countOfStreets = value;
                OnPropertyChanged(nameof(CountOfStreets));
            }
        }
        public DateTime? SelectedDateFromFilter
        {
            get { return _selectedDateFromFilter; }
            set
            {
                _selectedDateFromFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(SelectedDateFromFilter));
            }
        }

        public DateTime? SelectedDateToFilter
        {
            get { return _selectedDateToFilter; }
            set
            {
                _selectedDateToFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(SelectedDateToFilter));
            }
        }

        public Street SelectedStreet
        {
            get { return _selectedStreet; }
            set
            {
                _selectedStreet = value;
                OnPropertyChanged(nameof(IsStreetSelected));
                OnPropertyChanged(nameof(SelectedStreet));
            }
        }


        public ObservableCollection<Street> Streets
        {
            get { return _streets; }
            set
            {
                _streets = value;
                OnPropertyChanged(nameof(Streets));
            }
        }

        public bool IsActualFilter
        {
            get { return _isActualFilter; }
            set
            {
                _isActualFilter = value;
                LoadStreets();
                OnPropertyChanged(nameof(IsActualFilter));
            }
        }


        private readonly StreetService _streetService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        public StreetFrameViewModel(StreetService streetService, StreetNameHistoryService streetNameHistoryService)
        {
            _streetService = streetService;
            _streetNameHistoryService = streetNameHistoryService;
            LoadStreets();
            CountOfStreets = Streets.Count.ToString();
        }

        private void LoadStreets()
        {
            var streets = _streetService.GetStreetByFilters(NameSearchStreetFilter, IsActualFilter,
                IsHasSplitParentFilter, IsHasMergeParentsFilter,
                IsHasHistoryFilter,
                IsSortedByNameFilter,
                SelectedDateFromFilter.HasValue ?
                DateOnly.FromDateTime(SelectedDateFromFilter.Value) : null,
                SelectedDateToFilter.HasValue ?
                DateOnly.FromDateTime(SelectedDateToFilter.Value) : null);
            CountOfStreets = streets.Count().ToString();
            Streets = new ObservableCollection<Street>(streets);
        }
        public void AddParent(Street street, IEnumerable<Street> parents)
        {
            if(_streetService.GetParentStreetFromSplit(street) != null 
                && _streetService.GetParentStreetsFromMerge(street).Count() > 0)
            {
                MessageBox.Show("Вибрана вулиця уже має батьківські адреси");
                return;
            }
            else
            {
                //call window to select parent street
            }
        }

        public void AddChildrens(Street street, IEnumerable<Street> childrens)
        {
            if (_streetService.GetChildStreetFromMerge(street) != null
                && _streetService.GetChildStreetsFromSplit(street).Count()>0)
            {

            }
            else
            {
                //
            }
        }

        public void AddStreet()
        {
            new StreetCRWindow(_streetService, _streetNameHistoryService).ShowDialog();
            LoadStreets();
        }

        public void EditStreet(Street street)
        {
            new StreetCRWindow(_streetService, street).ShowDialog();
            LoadStreets();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
