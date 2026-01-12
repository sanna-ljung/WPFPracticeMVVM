using System.Configuration;
using System.Data;
using System.Windows;
using WpfPractice2.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WpfPractice2.Data;
using Microsoft.EntityFrameworkCore;
using WpfPractice2.Services;
using WpfPractice2.Models;

namespace WpfPractice2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            // Skapa DI-container
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Registrera DbContext
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(
                            "Server=(localdb)\\mssqllocaldb;Database=WpfPractice2Db;Trusted_Connection=True;"
                        )
                    );

                    // Registrera Service
                    services.AddScoped<IProductService, ProductService>();

                    // Registrera ViewModel
                    services.AddScoped<ProductViewModel>();

                    // Registrera MainWindow
                    services.AddSingleton<MainWindow>();
                })
                .Build();

            Startup += App_Startup;
        }

        private async void App_Startup(object sender, StartupEventArgs e)
        {
            // Sätt MainWindow och visa den
            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            // Hämta ViewModel och ladda data
            var viewModel = AppHost.Services.GetRequiredService<ProductViewModel>();
            mainWindow.DataContext = viewModel;
            await viewModel.LoadAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            AppHost?.Dispose();
            base.OnExit(e);
        }
    }

}
