using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public AdressFrame(AdressService service, StreetService streetService)
        {
            InitializeComponent();
            _adressService = service;
            _viewModel = new AdressViewModel(service, streetService);
            DataContext = _viewModel;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteAdress(_viewModel.SelectedAdress);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //_viewModel.AddAdress();
        }

        private void HistoryAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdressDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedAdress.AdressRecords.Count < 1) 
            { 
                MessageBox.Show("У адреси немає історичних записів"); 
                return; 
            }
            new AdressHistory(_viewModel.SelectedAdress, _adressService).ShowDialog();
        }
    }
}
