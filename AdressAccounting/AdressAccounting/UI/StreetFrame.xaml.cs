using System;
using System.Collections.Generic;
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
    /// Interaction logic for StreetFrame.xaml
    /// </summary>
    public partial class StreetFrame : Page
    {


        private readonly StreetService _streetService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        private readonly StreetFrameViewModel _viewModel;
        public StreetFrame(StreetService streetService,StreetNameHistoryService streetNameHistoryService)
        {
            _streetService = streetService;
            _streetNameHistoryService = streetNameHistoryService;
            _viewModel = new StreetFrameViewModel(_streetService, _streetNameHistoryService);
            InitializeComponent();
            this.DataContext = _viewModel;
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddStreet();


        }

        private void HistoryAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.EditStreet(_viewModel.SelectedStreet);
        }

        private void AdressDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (new StreetDataWindow(_viewModel.SelectedStreet, _streetService, _streetNameHistoryService)).ShowDialog();
        }

        private void AddParentsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
