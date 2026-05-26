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

        public Street SelectedStreet
        {
            get { return _selectedStreet; }
            set
            {
                _selectedStreet = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Street> Streets
        {
            get { return _streets; }
            set
            {
                _streets = value;
                OnPropertyChanged();
            }
        }


        private readonly StreetService _streetService;
        public StreetFrameViewModel(StreetService streetService)
        {
            _streetService = streetService;
            Streets = new ObservableCollection<Street>(_streetService.GetAllStreets());
        }

        public void AddParent(Street street, IEnumerable<Street> parents)
        {
            if(_streetService.GetParentStreetFromSplit(street) != null 
                || _streetService.GetParentStreetsFromMerge(street).Count() > 0)
            {
                MessageBox.Show("Вибрана вулиця уже має батьківські адреси");
                return;
            }
            else
            {
                //call window to select parent street
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
