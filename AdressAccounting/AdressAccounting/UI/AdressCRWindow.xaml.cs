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
using static AdressAccounting.UI.AdressRenumeringWindow;

namespace AdressAccounting.UI
{
    /// <summary>
    /// Interaction logic for AdressCRWindow.xaml
    /// </summary>
    public partial class AdressCRWindow : Window
    {

        private readonly AdressCRwindowViewModel _viewModel;
        private bool _isUpdateMode;

        public AdressCRWindow(AdressService service, StreetService streetService)
        {
            _viewModel = new AdressCRwindowViewModel(service, streetService);
            InitializeComponent();
            this.DataContext = _viewModel;
        }

        private Adress _adress;
        private bool _isHistoryMode;
        public AdressCRWindow(AdressService service, StreetService streetService, Adress adress, 
            bool isUpdateMode = false, bool isHistoryMode = false)
        {
            _viewModel = new AdressCRwindowViewModel(service, streetService, adress,isHistoryMode);
            InitializeComponent();
            ActualCheckBox.Visibility = Visibility.Collapsed;
            this._isUpdateMode = isUpdateMode;
            _adress = adress;
            if (this._isUpdateMode){
                AddButton.Content = "Оновити";
                DateFromGrid.Visibility = Visibility.Collapsed;
            }
            if(isHistoryMode)
            {
                AddButton.Content = "Додати історичний запис";
                _isHistoryMode = true;
            }
            this.DataContext = _viewModel;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._isUpdateMode){
                bool isSaved = await _viewModel.UpdateAdress(_adress);
                if (isSaved)
                {
                    this.DialogResult = true;
                    this.Close();
                    return;
                }
            }
            if (this._isHistoryMode)
            {
                bool isSaved = await _viewModel.AddHistory(_adress);
                if (isSaved)
                {
                    this.DialogResult = true;
                    this.Close();
                    return;
                }
            }
            if(await _viewModel.CreateAdress())
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
