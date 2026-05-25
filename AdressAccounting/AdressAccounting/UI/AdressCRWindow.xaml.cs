using AdressAccounting.Validators;
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
using System.Windows.Shapes;

namespace AdressAccounting.UI
{
    /// <summary>
    /// Interaction logic for AdressCRWindow.xaml
    /// </summary>
    public partial class AdressCRWindow : Window
    {

        private readonly AdressCRwindowViewModel _viewModel;

        public AdressCRWindow(AdressService service, StreetService streetService)
        {
            _viewModel = new AdressCRwindowViewModel(service, streetService);
            InitializeComponent();
            this.DataContext = _viewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(await _viewModel.CreateAdress())
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
