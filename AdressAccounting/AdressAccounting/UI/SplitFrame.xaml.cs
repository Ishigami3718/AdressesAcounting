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
    /// Interaction logic for SplitFrame.xaml
    /// </summary>
    public partial class SplitFrame : Page
    {
        private readonly SplitFrameViewModel _viewModel;
        private readonly SplitService _service;
        private readonly StreetService _streetService;
        public SplitFrame(SplitService service, StreetService streetService)
        {
            InitializeComponent();
            _service = service;
            _streetService = streetService;
            _viewModel = new SplitFrameViewModel(_service, _streetService);
            this.DataContext = _viewModel;

        }

        private void Streets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedStreetFromAll != null)
                _viewModel.SwapStreetDirect(_viewModel.SelectedStreetFromAll);

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedStreetFromMerging != null)
                _viewModel.SwapStreetReverse(_viewModel.SelectedStreetFromMerging);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GetNewNumbers();
        }
    }
}
