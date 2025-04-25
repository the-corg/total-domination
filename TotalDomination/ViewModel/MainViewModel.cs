using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
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

            var todos = new List<TodoViewModel>();
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 01. This is task number 01" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 02. This is task number 02" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 03. This is task number 03" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 04. This is task number 04" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 05. This is task number 05" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 06. This is task number 06" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 07. This is task number 07" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 08. This is task number 08" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 09. This is task number 09" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 10. This is task number 10" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 11. This is task number 11" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 12. This is task number 12" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 13. This is task number 13" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 14. This is task number 14" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 15. This is task number 15" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 16. This is task number 16" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 17. This is task number 17" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 18. This is task number 18" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 19. This is task number 19" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 20. This is task number 20" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 21. This is task number 21" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 22. This is task number 22" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 23. This is task number 23" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 24. This is task number 24" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 25. This is task number 25" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 26. This is task number 26" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 27. This is task number 27" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 28. This is task number 28" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 29. This is task number 29" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 30. This is task number 30" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 31. This is task number 31" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 32. This is task number 32" }, _calculations));
            todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 33. This is task number 33" }, _calculations));
            //todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 34. This is task number 34" }, _calculations));
            //todos.Add(new TodoViewModel(new Todo() { Added = DateOnly.MinValue, FileName = _todoFilePath, Title = "This is task number 35. This is task number 35" }, _calculations));

            for (int i = 0; i < todos.Count; i++)
            {
                todos[todos.Count - 1 - i].DaysSinceDone = 7*i + 1;
            }

            _calculations.TotalFrequency = todos.Sum(x => x.Frequency);

            foreach (var todo in todos)
                Todos.Add(todo);





            /*
            var white = new Color() { A = 255, R = 255, G = 255, B = 255 };
            var red = new Color() { A = 255, R = 255, G = 0, B = 0 };
            var goodRed = new Color() { A = 255, R = 255, G = 64, B = 64 };
            var c1 = new Color() { A = 255, R = 255, G = 0, B = 0 };
            var c2 = new Color() { A = 255, R = 0, G = 255, B = 0 };
            */

            // Custom interpolation algorithm

            // Compensation for the darkness of the red
            /*var totalFrequency = 16;
            var delta = 2.0 / totalFrequency;
            for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                // -(x-3)^2 + 9, where x changes from 0 to 2
                var xminus3 = i * delta - 3;
                var quadraticCurvePart = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;
                c1.G = (byte)quadraticCurvePart;
                c1.B = 0;
                c2.R = (byte)quadraticCurvePart;

                if (quadraticCurvePart < 160.0)
                {
                    c1.G = (byte)((160.0 + quadraticCurvePart) / 2);
                    c1.B = (byte)((160.0 - quadraticCurvePart) / 2);
                }

                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;



            }*/


            //Everything at once
            /*var totalFrequency = 8;
            var delta = 2.0 / totalFrequency;
            for (int i = 0; i < Todos.Count / 4 + 1; i++)
            {
                // -(x-3)^2 + 9, where x changes from 0 to 2
                var xminus3 = i * delta - 3;
                var quadraticCurvePart = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;
                c1.G = (byte)quadraticCurvePart;
                c1.B = 0;
                c2.R = (byte)quadraticCurvePart;

                

                if (quadraticCurvePart < 128.0)
                {
                    c1.G = (byte)((128.0 + quadraticCurvePart) / 2);
                    c1.B = (byte)((128.0 - quadraticCurvePart) / 2);
                }

                Todos[i + 16].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;


            }

            for (int i = 0; i < 16; i++) Todos[i].Color = goodRed;

            for (int i = 0; i <= 4; i++) Todos[i].FontWeight = FontWeights.UltraBold;
            for (int i = 5; i <= 8; i++) Todos[i].FontWeight = FontWeights.Bold;
            for (int i = 9; i <= 12; i++) Todos[i].FontWeight = FontWeights.DemiBold;
            for (int i = 0; i <= 12; i++) Todos[i].Title = "🔥" + Todos[i].Title;
            for (int i = 0; i <= 8; i++) Todos[i].Title = "🔥" + Todos[i].Title;
            for (int i = 0; i <= 4; i++) Todos[i].Title = "🔥" + Todos[i].Title;




            Todos[0].Title = "🔥" + Todos[0].Title;*/

            /*for (int i = 0; i < 16; i++)
{
    red.B = (byte)(red.B + 4);
    red.G = (byte)(red.G + 4);
    Todos[15 - i].Color = red;

}*/


            /*for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                // -(x-3)^2 + 9, where x changes from 0 to 2
                var xminus3 = i * delta - 3;
                var quadraticCurvePart = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;
                c1.G = (byte)quadraticCurvePart;
                c1.B = 0;
                c2.R = (byte)quadraticCurvePart;

                if (quadraticCurvePart < 255.0)
                {
                    c1.G = (byte)((128.0 + quadraticCurvePart) / 2);
                    c1.B = (byte)((128.0 - quadraticCurvePart) / 2);
                }

                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;



            }
            */


            // BEST
            /*
            var totalFrequency = 16;
            var delta = 2.0 / totalFrequency;
            for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                // -(x-3)^2 + 9, where x changes from 0 to 2
                var xminus3 = i * delta - 3;
                var quadraticCurvePart = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;
                c1.G = (byte)quadraticCurvePart;
                c2.R = (byte)quadraticCurvePart;

                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;
            }
            */

            /* Custom interpolation algorithm - still not enough yellow
            var delta = 1.0 / 16;
            for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                var xminus2 = i * delta - 2;

                c1.G = (byte)(255 * (4.0 - xminus2 * xminus2) / 3.0);
                c2.R = (byte)(255 * (4.0 - xminus2 * xminus2) / 3.0);

                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;


            }*/


            /*sRGB
            for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;
                c1.G = (byte)(c1.G + 16);
                c2.R = (byte)(c2.R + 16);
            }
            Todos[16].Color = new Color() { A = 255, R = 255, G = 255, B = 0 };*/

            /*scRGB (something like linear RGB) 
            for (int i = 0; i < Todos.Count / 2 + 1; i++)
            {
                Todos[i].Color = c1;
                Todos[Todos.Count - 1 - i].Color = c2;
                c1.ScG = c1.ScG + 1.0f/16;
                c2.ScR = c2.ScR + 1.0f / 16;
            }*/


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
