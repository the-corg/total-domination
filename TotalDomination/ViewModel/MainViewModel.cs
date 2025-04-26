using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TotalDomination.Data;
using TotalDomination.Model;
using TotalDomination.Properties;
using TotalDomination.Utilities;

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

        private List<Todo> _completeList = new();
        private List<Todo> _currentList = new();

        #endregion

        #region Constructor, Initializer and SaveData

        public MainViewModel(FileManager fileManager, Calculations calculations)
        {
            _fileManager = fileManager;
            _calculations = calculations;

            SelectFileCommand = new DelegateCommand(async _ => await SelectFileAsync());
            DoneCommand = new DelegateCommand(async param => await DoneAsync(param));
        }

        /// <summary>
        /// Initializes the data
        /// </summary>
        public async Task InitializeAsync()
        {
            // First load the complete to-do list
            _completeList = await _fileManager.LoadCompleteListAsync();

            if (string.IsNullOrWhiteSpace(_todoFilePath))
                return;

            // Then load the current to-do list
            if (File.Exists(_todoFilePath))
            {
                await LoadAndProcessCurrentListAsync();
            }
            else
            {
                MessageBox.Show("Could not find file: " + _todoFilePath, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _todoFilePath = "";
                OnPropertyChanged(nameof(TodoFileName));
            }

        }

        /// <summary>
        /// Saves the list of to-do items 
        /// </summary>
        public async Task SaveDataAsync()
        {
            await _fileManager.SaveCompleteListAsync(_completeList);
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
        public string TodoFileName => string.IsNullOrWhiteSpace(_todoFilePath) ? "Open File" : Path.GetFileNameWithoutExtension(_todoFilePath);

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
        private async Task SelectFileAsync()
        {
            var newTodoFilePath = _fileManager.SelectTodoListFile();

            if (string.IsNullOrWhiteSpace(newTodoFilePath) || newTodoFilePath == _todoFilePath)
                return;

            _todoFilePath = newTodoFilePath;
            Settings.Default.TodoListFile = _todoFilePath;
            Settings.Default.Save();
            OnPropertyChanged(nameof(TodoFileName));

            await LoadAndProcessCurrentListAsync();
        }

        /// <summary>
        /// Command for showing the success message
        /// </summary>
        public DelegateCommand DoneCommand { get; }
        private async Task DoneAsync(object? parameter)
        {
            if (parameter is bool isDone)
            {
                if (isDone)
                {
                    CanCelebrate = true; // The setter fires PropertyChanged, and the animation starts

                    CanCelebrate = false; // That's it. Enough celebrating
                }

                await SaveDataAsync();
            }
        }
        #endregion

        #region Private methods 

        /// <summary>
        /// Loads todo file, modifies complete list, saves it, creates TodoViewModels
        /// </summary>
        private async Task LoadAndProcessCurrentListAsync()
        {
            _currentList = await _fileManager.LoadCurrentListAsync(_todoFilePath);

            // Add anything that's new to the complete list
            for (int i = 0; i < _currentList.Count; i++)
            {
                var matchingTodo = _completeList.FirstOrDefault(x => (x.FileName == _currentList[i].FileName) && (x.Title == _currentList[i].Title));

                if (matchingTodo is not null)
                {
                    // There's already such a to-do item in the complete list

                    // Update frequency, if needed
                    if (matchingTodo.Frequency != _currentList[i].Frequency)
                    {
                        matchingTodo.Frequency = _currentList[i].Frequency;
                    }

                    // Set the reference to the object from the complete list
                    _currentList[i] = matchingTodo;
                }
                else
                {
                    // This to-do item is new
                    _completeList.Add(_currentList[i]);
                }
            }

            // Back up and save complete list
            _fileManager.MakeBackup();
            await SaveDataAsync();

            _calculations.TotalFrequency = _currentList.Sum(x => x.Frequency);

            // Create todo view models
            var todoViewModels = _currentList.Select(x => new TodoViewModel(x, _calculations)).ToList();

            Todos.Clear();
            foreach (var t in todoViewModels)
                Todos.Add(t);
        }

        #endregion

    }
}
