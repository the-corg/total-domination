using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TotalDomination.Data;
using TotalDomination.ViewModel;

namespace TotalDomination
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private fields and the constructor 
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        #endregion

        #region Configure services for dependency injection
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<Calculations>();
            services.AddTransient<FileManager>();
        }
        #endregion

        #region OnStartup (show MainWindow)
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show the main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow?.Show();
        }
        #endregion

    }

}
