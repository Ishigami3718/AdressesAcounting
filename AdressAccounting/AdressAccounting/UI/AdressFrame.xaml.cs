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
        public AdressFrame(AdressService service, StreetService streetService)
        {
            InitializeComponent();
            _viewModel = new AdressViewModel(service, streetService);
            DataContext = _viewModel;
        }
    }
}
