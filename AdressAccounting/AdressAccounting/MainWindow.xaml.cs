using AdressAccounting.Services;
using System.Diagnostics;
using System.Net;
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

namespace AdressAccounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AdressAccountingContext _db;
        private readonly MergeService _mergeService;
        private readonly StreetService _streetService;
        private readonly SplitService _splitService;
        private readonly StreetNameHistoryService _streetNameHistoryService;
        private readonly AdressService _adressService;
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _db = Db.Context;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка підключення бази даних: " + ex.Message);
                return;
            }
            try
            {
                _mergeService = new MergeService(_db);
                _splitService = new SplitService(_db);
                _streetService = new StreetService(_db);
                _adressService = new AdressService(_db);
                _streetNameHistoryService = new StreetNameHistoryService(_db);
                MainFrame.Navigate(new UI.AdressFrame(_adressService, _streetService, _streetNameHistoryService));
            }
            catch(Exception ex) { MessageBox.Show("Помилка підключення бази даних: " + ex.Message);return; }

            /*foreach( var street in streetService.GetParentStreetsFromMerge(db.Streets.ToList()[2]))
            {
                Debug.WriteLine($"{street.Id}     {street.Name}");
            }*/

            }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void MenuItemAdresses_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UI.AdressFrame(_adressService, _streetService, _streetNameHistoryService));
        }

        private void MenuItemStreets_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UI.StreetFrame(_streetService, _streetNameHistoryService));
        }

        private void MenuItemMerging_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UI.MergeFrame(_mergeService, _streetService));
        }

    }
}