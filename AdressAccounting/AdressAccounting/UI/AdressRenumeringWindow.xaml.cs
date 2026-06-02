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
    /// Interaction logic for AdressRenumeringWindow.xaml
    /// </summary>
    public partial class AdressRenumeringWindow : Window
    {
        private List<Street> _newstreets = new List<Street>();
        public ObservableCollection<Adress> Adresses { get; set; }
        public ObservableCollection<Street> Streets { get; set; }
        public struct NewNumber
        {
            public int Number {  get; set; }
            public Street Street { get; set; }
        }

        public ObservableCollection<NewNumber> NewNumbers_ {  get; set; } = new ObservableCollection<NewNumber>();
        
        public int[] NewNumbersInt => NewNumbers_.Select(x => x.Number).ToArray();
        public AdressRenumeringWindow(ICollection<Street> streets)
        {
            InitializeComponent();
            Adresses = new ObservableCollection<Adress>();
            foreach(var street in streets)
            {
                foreach (var adress in street.Adresses)
                {
                    if ((bool)adress.IsActual) 
                    { 
                        Adresses.Add(adress);
                        NewNumbers_.Add(new NewNumber());
                    }
                }
            }
            this.DataContext = this;

        }

        public AdressRenumeringWindow(Street street, ICollection<Street> streets)
        {
            InitializeComponent();
            Streets = new ObservableCollection<Street>(streets);
            Adresses = new ObservableCollection<Adress>(street.Adresses.Where(a => (bool)a.IsActual));
            NewStreetComboBox.Visibility = Visibility.Visible;
            AdressesColumn.Width = new GridLength(0.7,GridUnitType.Star);
            NewNumbersColumn.Width = new GridLength(0.3,GridUnitType.Star);
            NewNumbers_ = new(); 
            Enumerable.Range(0, Adresses.Count).ToList().ForEach(i => NewNumbers_.Add(new NewNumber()));
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
