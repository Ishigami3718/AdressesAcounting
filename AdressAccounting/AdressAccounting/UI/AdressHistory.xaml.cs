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
    /// Interaction logic for AdressHistory.xaml
    /// </summary>
    public partial class AdressHistory : Window
    {

        public ObservableCollection<AdressRecord> AdressRecords { get; set; }

        private AdressService _service;
        public AdressHistory(IQueryable<AdressRecord> adressRecords, AdressService adressService)
        {
            InitializeComponent();
            _service = adressService;
            this.AdressRecords = new ObservableCollection<AdressRecord>(adressRecords);
            this.DataContext = this;
        }
    }
}
