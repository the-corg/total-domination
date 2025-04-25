using System.IO;
using System.Text.Json;
using System.Windows;
using Microsoft.Win32;
using TotalDomination.Model;
using TotalDomination.Utilities;

namespace TotalDomination.Data
{
    /// <summary>
    /// Performs file system operations
    /// </summary>
    public class FileManager
    {
        #region Private fields 
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        private static readonly string _appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TotalDomination");
        private readonly string _jsonFilePath = Path.Combine(_appDataFolder, "TotalList.dat");

        #endregion

        #region Select To-do list file

        const string _filterTxt = "Text files (*.txt)|*.txt";
        const string _filterAll = "All files (*.*)|*";

        /// <summary>
        /// Lets the user select a text file using OpenFileDialog
        /// </summary>
        /// <returns>File name if successfull<br\>null, otherwise</returns>
        public string? SelectTodoListFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select the pipe-delimited text file with your To-do list",
                Filter = _filterTxt + "|" + _filterAll
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        #endregion

        #region Load lists 

        /// <summary>
        /// Loads the complete list of to-do items from the JSON file
        /// </summary>
        /// <returns>Complete list if all to-do items, including obsolete ones</returns>
        public async Task<List<Todo>> LoadCompleteListAsync()
        {
            if (File.Exists(_jsonFilePath))
            {
                string json = await File.ReadAllTextAsync(_jsonFilePath);

                return JsonSerializer.Deserialize<List<Todo>>(json) ?? [];
            } 
            else
                return new List<Todo>();
        }

        /// <summary>
        /// Loads the list of current to-do items from the pipe-delimited txt file
        /// </summary>
        /// <returns>List of current to-do items</returns>
        public async Task<List<Todo>> LoadCurrentListAsync(string todoFilePath)
        {
            var list = new List<Todo>();

            if (File.Exists(todoFilePath))
            {
                try
                {
                    using (var reader = File.OpenText(todoFilePath))
                    {
                        var fileText = await reader.ReadToEndAsync();
                        var lines = fileText.Split(Environment.NewLine);
                        var fileName = Path.GetFileName(todoFilePath);
                        var today = Calculations.GetTodayWithMidnightShift();

                        var data = from line in lines
                                   let split = line.Split('|')
                                   where split.Length == 2
                                   select new Todo
                                   {
                                       FileName = fileName,
                                       Title = split[1],
                                       Frequency = int.Parse(split[0]),
                                       Added = today
                                   };
                        list.AddRange(data);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error reading pipe-delimited txt-file: \n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

            return list;
        }
        #endregion

        #region Save list to json 

        /// <summary>
        /// Saves the complete list of to-do items to the JSON file
        /// </summary>
        /// <param name="todos">The complete list of to-do items</param>
        public async Task SaveCompleteListAsync(List<Todo> todos)
        {
            Directory.CreateDirectory(_appDataFolder); // Ensure that the folder exists

            string json = JsonSerializer.Serialize(todos, jsonSerializerOptions);

            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
        #endregion


    }


}
