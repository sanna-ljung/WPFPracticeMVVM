using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPractice2.Command;
using WpfPractice2.Models;
using WpfPractice2.ViewModels;

namespace WpfPractice2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProductViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ProductViewModel();
            DataContext = viewModel;
            Loaded += ProductsView_Loaded;
        }

        private async void ProductsView_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.LoadAsync();
        }

    }
}