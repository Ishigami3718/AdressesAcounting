using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdressAccounting.UI
{
    /// <summary>
    /// Interaction logic for AdressFrame.xaml
    /// </summary>
    public partial class AdressFrame : Page
    {
        private readonly AdressViewModel _viewModel;
        private readonly AdressService _adressService;
        private readonly StreetService _streetService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        public AdressFrame(AdressService service, StreetService streetService, StreetNameHistoryService streetNameHistoryService)
        {
            InitializeComponent();
            _adressService = service;
            _streetService = streetService;
            _streetNameHistoryService = streetNameHistoryService;
            _viewModel = new AdressViewModel(service, streetService);
            DataContext = _viewModel;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Видалення призведе до повної втрати даних, " +
                "рекомендується видаляти лише, якщо створено абсолютно неправильний об'єкт без можливості відредагування", 
                "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _viewModel.DeleteAdress(_viewModel.SelectedAdress);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddAdress();
            Debug.WriteLine("Add button clicked");
        }

        private void HistoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddHistory(_viewModel.SelectedAdress);
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.UpdateAdress();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdressDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var adressRecords = _adressService.GetAdressHistory(_viewModel.SelectedAdress);
            if (adressRecords.Count() <= 1) 
            { 
                MessageBox.Show("У адреси немає історичних записів"); 
                return; 
            }
            new AdressHistory(adressRecords, _adressService).ShowDialog();
        }

        private void AddSecondAdressOnArea_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddAdressOnSameArea();
        }
    }
}
