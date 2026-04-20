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
        public MainWindow()
        {
            InitializeComponent();
            AdressAccountingContext db = Db.Context;
            MergeService mergeService = new MergeService(db);
            StreetService streetService = new StreetService(db);

            foreach( var street in streetService.GetParentStreetsFromMerge(db.Streets.ToList()[2]))
            {
                Debug.WriteLine($"{street.Id}     {street.Name}");
            }
            
        }
    }
}