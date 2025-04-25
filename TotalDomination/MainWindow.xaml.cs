using System.Windows;
using System.Windows.Input;
using TotalDomination.ViewModel;

namespace TotalDomination
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor and private fields 
        private bool _readyToBeClosed = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Handling window state changes
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Increase the window border thickness for the maximized state,
                // otherwise the window would extend beyond the screen edges
                BorderThickness = new Thickness(8);
                MaximizeButton.Visibility = Visibility.Collapsed;
                RestoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

        private void CommandBinding_MinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CommandBinding_MaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void CommandBinding_RestoreExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void CommandBinding_CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Handlers for major events 

        // Calls the view model's initializer
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                await mainViewModel.InitializeAsync();
            }
        }

        // Saves data on exit
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_readyToBeClosed)
                return; // Closes the window

            e.Cancel = true; // Cancel closing for now
            Hide(); // Hide the window immediately

            if (DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.SaveData();
            }

            // Now really close the window
            _readyToBeClosed = true;
            Close();
        }
        #endregion
    }
}