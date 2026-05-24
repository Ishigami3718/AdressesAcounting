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
            _db = Db.Context;
            _mergeService = new MergeService(_db);
            _splitService = new SplitService(_db);
            _streetService = new StreetService(_db);
            _adressService = new AdressService(_db);
            _streetNameHistoryService = new StreetNameHistoryService(_db);
            MainFrame.Navigate(new UI.AdressFrame(_adressService, _streetService));

            /*foreach( var street in streetService.GetParentStreetsFromMerge(db.Streets.ToList()[2]))
            {
                Debug.WriteLine($"{street.Id}     {street.Name}");
            }*/

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}