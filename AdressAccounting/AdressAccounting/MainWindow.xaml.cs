using AdressAccounting.Services;
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
            mergeService.MergeStreets(new List<Street> 
            {  
                db.Streets.First(), 
                 db.Streets.Skip(1).First()
            }, 
            new Street
            {
                Name = "3"
            }, 
            DateOnly.FromDateTime(DateTime.Now));
        }
    }
}