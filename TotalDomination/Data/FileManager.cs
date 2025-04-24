using Microsoft.Win32;

namespace TotalDomination.Data
{
    /// <summary>
    /// Performs file system operations
    /// </summary>
    public class FileManager
    {

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



    }


}
