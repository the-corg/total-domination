using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TotalDomination.Data;
using TotalDomination.Model;
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
        private readonly Calculations _calculations;
        private string _todoFilePath = Settings.Default.TodoListFile;

        #endregion

        #region Constructor and Initializer

        public MainViewModel(FileManager fileManager, Calculations calculations)
        {
            _fileManager = fileManager;
            _calculations = calculations;

            SelectFileCommand = new DelegateCommand(execute => SelectFile());
        }

        /// <summary>
        /// Initializes the data
        /// </summary>
        public async Task InitializeAsync()
        {
            // First load JSON (async)
            // If there's no JSON, create an empty one

            if (string.IsNullOrEmpty(_todoFilePath))
                return;

            if (File.Exists(_todoFilePath))
            {
                // Load file here (async)
                // Add everything that's not in JSON to JSON
            }
            else
            {
                MessageBox.Show("Could not find file: " + _todoFilePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _todoFilePath = "";
                OnPropertyChanged(nameof(TodoFileName));
            }

            // TODO: remove this later
            var todos = new List<TodoViewModel>();
            for (int i = 0; i < 33; i++)
                todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number " + (i + 1) }, _calculations));
            for (int i = 0; i < todos.Count; i++)
                todos[todos.Count - 1 - i].DaysSinceDone = 7*i + 1;

            _calculations.TotalFrequency = todos.Sum(x => x.Frequency);

            foreach (var todo in todos)
                Todos.Add(todo);
        }

        #endregion


        #region Public properties 

        public ObservableCollection<TodoViewModel> Todos { get; set; } = new();

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
            _todoFilePath = _fileManager.SelectTodoListFile() ?? "";
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
