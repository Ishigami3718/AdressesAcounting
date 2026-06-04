using AdressAccounting.Validators;
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
    /// Interaction logic for MergeFrame.xaml
    /// </summary>
    public partial class MergeFrame : Page
    {
        private readonly MergeFrameViewModel _viewModel;
        private readonly StreetService _streetService;
        private readonly StreetValidator _streetValidator;
        public MergeFrame(MergeService mergeService, StreetService service)
        {
            InitializeComponent();
            _streetService = service;
            _viewModel = new MergeFrameViewModel(mergeService, _streetService);
            this.DataContext = _viewModel;
        }

        private void Streets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(_viewModel.SelectedStreetFromAll != null) 
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.MergeStreets();
        }
    }
}
