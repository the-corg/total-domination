using System.Windows;
using System.Windows.Input;
using TotalDomination.Properties;
using TotalDomination.ViewModel;

namespace TotalDomination
{
    /// <summary>
    /// MVVM-friendly code-behind for MainWindow.xaml.
    /// For the most part, it's strictly view-related code.
    /// Additionally, two DataContext methods are called, to handle 
    /// startup (initialize data asynchronously after the UI is loaded)
    /// and exit (to save data asynchronously on exit) to avoid creating 
    /// extra complexity of attached behaviors for silly reasons.
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

        #region Startup/closing events 

        // Restores saved window size, calls the view model's initializer
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Restore the window position, size, and state
            Top = Settings.Default.MainWindowTop;
            Left = Settings.Default.MainWindowLeft;
            Height = Settings.Default.MainWindowHeight;
            Width = Settings.Default.MainWindowWidth;
            WindowState = Settings.Default.MainWindowState;

            if (DataContext is MainViewModel mainViewModel)
            {
                await mainViewModel.InitializeAsync();
            }
        }

        // Calls the view model to save data on exit, saves window size 
        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_readyToBeClosed)
                return; // Closes the window

            e.Cancel = true; // Cancel closing for now
            Hide(); // Hide the window immediately

            if (DataContext is MainViewModel mainViewModel)
            {
                await mainViewModel.SaveDataAsync();
            }

            // Save the window position, size, and state
            Settings.Default.MainWindowTop = RestoreBounds.Top;
            Settings.Default.MainWindowLeft = RestoreBounds.Left;
            Settings.Default.MainWindowHeight = RestoreBounds.Height;
            Settings.Default.MainWindowWidth = RestoreBounds.Width;
            Settings.Default.MainWindowState = WindowState;
            Settings.Default.Save();

            // Now really close the window
            _readyToBeClosed = true;
            Close();
        }
        #endregion
    }
}