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
using System.Windows.Shapes;

namespace AdressAccounting.UI
{
    /// <summary>
    /// Interaction logic for StreetCRWindow.xaml
    /// </summary>
    public partial class StreetCRWindow : Window
    {
        private StreetCRWindowViewModel _viewModel;
        private bool _isRedact = false;
        public StreetCRWindow(StreetService streetService, StreetNameHistoryService streetNameHistoryService)
        {
            InitializeComponent();
            _viewModel = new StreetCRWindowViewModel(streetService, streetNameHistoryService);
            DataContext = _viewModel;
        }

        public StreetCRWindow(StreetService streetService,Street street)
        {
            InitializeComponent();
            _viewModel = new StreetCRWindowViewModel(streetService, street);
            DataContext = _viewModel;
            IsActualCheckBox.Visibility = Visibility.Collapsed;
            AddButton.Content = "Редагувати";
            _isRedact = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CreateRedactStreet(_isRedact)) this.Close();
            else return;
        }
    }
}
