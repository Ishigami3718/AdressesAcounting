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
    /// Interaction logic for StreetDataWindow.xaml
    /// </summary>
    public partial class StreetDataWindow : Window
    {
        public Street Street { get; set; }
        public ObservableCollection<Street> ParentStreets { get; set; }
        public ObservableCollection<Street> ChildStreets { get; set; }

        public ObservableCollection<StreetNameRecord> StreetNameRecords { get; set; }

        private readonly StreetService _streetService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        public StreetDataWindow(Street street, StreetService streetService, 
            StreetNameHistoryService streetNameHistoryService)
        {
            InitializeComponent();
            Street = street;
            _streetService = streetService;
            _streetNameHistoryService = streetNameHistoryService;
            ParentStreets = new ObservableCollection<Street>(_streetService.GetParentStreets(Street));
            ChildStreets = new ObservableCollection<Street>(_streetService.GetChildStreets(Street));
            StreetNameRecords = new ObservableCollection<StreetNameRecord>(
                _streetNameHistoryService.GetNameHistory(Street));
            this.DataContext = this;
        }
    }
}
