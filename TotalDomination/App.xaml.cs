using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TotalDomination.ViewModel;

namespace TotalDomination
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        #region Configure services for dependency injection
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            // View Models
            services.AddSingleton<MainViewModel>();

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
