using System.IO;
using TotalDomination.Data;
using TotalDomination.Properties;

namespace TotalDomination.ViewModel
{
    /// <summary>
    /// View Model for Main Window
    /// </summary>
    public class MainViewModel : BaseViewModel
    {

        #region Private fields

        private readonly FileManager _fileManager;
        private string _todoFilePath = Settings.Default.TodoListFile;

        #endregion

        #region Constructor and Initializer

        public MainViewModel(FileManager fileManager)
        {
            _fileManager = fileManager;

            SelectFileCommand = new DelegateCommand(execute => SelectFile());
        }

        /// <summary>
        /// Initializes the data
        /// </summary>
        public async Task InitializeAsync()
        {
            if (File.Exists(_todoFilePath))
            {

            }
        }

        #endregion


        #region Public properties 

        /// <summary>
        /// Name of the To-do list file
        /// </summary>
        public string TodoFileName => string.IsNullOrWhiteSpace(_todoFilePath) ? "Open File" : Path.GetFileName(_todoFilePath);


        #endregion

        #region Commands and their delegates
        /// <summary>
        /// Command for selecting the To-do list file
        /// </summary>
        public DelegateCommand SelectFileCommand { get; }

        private void SelectFile()
        {
            _todoFilePath = _fileManager.SelectTodoListFile();
            Settings.Default.TodoListFile = _todoFilePath;
            Settings.Default.Save();
            OnPropertyChanged(nameof(TodoFileName));
        }
        #endregion


        #region Success message

        private bool _isSuccessVisible;

        /// <summary>
        /// Determines whether the success message should be visible currently
        /// </summary>
        public bool IsSuccessVisible
        {
            get => _isSuccessVisible;
            set
            {
                if (_isSuccessVisible == value)
                    return;

                _isSuccessVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the property for the success animation for a while 
        /// </summary>
        public async Task ShowSuccessAsync()
        {
            IsSuccessVisible = true;
            await Task.Delay(2000);
            IsSuccessVisible = false;
        }
        #endregion


    }
}
