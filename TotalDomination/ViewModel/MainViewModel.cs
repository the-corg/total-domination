using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private bool _canCelebrate;

        #endregion

        #region Constructor and Initializer

        public MainViewModel(FileManager fileManager, Calculations calculations)
        {
            _fileManager = fileManager;
            _calculations = calculations;

            SelectFileCommand = new DelegateCommand(execute => SelectFile());
            DoneCommand = new DelegateCommand(Done);
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

        /// <summary>
        /// The list of to-do items 
        /// </summary>
        public ObservableCollection<TodoViewModel> Todos { get; set; } = new();

        /// <summary>
        /// Name of the To-do list file
        /// </summary>
        public string TodoFileName => string.IsNullOrWhiteSpace(_todoFilePath) ? "Open File" : Path.GetFileName(_todoFilePath);

        /// <summary>
        /// Shows whether celebration of success would be appropriate at the moment
        /// </summary>
        public bool CanCelebrate
        {
            get => _canCelebrate;
            set
            {
                if (_canCelebrate == value)
                    return;

                _canCelebrate = value;
                OnPropertyChanged();
            }
        }
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

        /// <summary>
        /// Command for showing the success message
        /// </summary>
        public DelegateCommand DoneCommand { get; }
        private void Done(object? parameter)
        {
            if (parameter is bool isChecked)
            {
                if (isChecked)
                {
                    CanCelebrate = true; // The setter fires PropertyChanged, and the animation starts

                    CanCelebrate = false; // That's it. Enough celebrating
                }
            }
        }
        #endregion


    }
}
