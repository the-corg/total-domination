namespace TotalDomination.Model
{
    /// <summary>
    /// Model class for a To-do item
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// File name of the To-do list, from which the to-do item was loaded
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// The title of the To-do item
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// The frequency of the To-do item (used for sorting by urgency)
        /// </summary>
        public int Frequency { get; set; } = 1;

        /// <summary>
        /// The date when the to-do item was first loaded from a To-do list file
        /// </summary>
        public required DateOnly Added { get; set; }

        /// <summary>
        /// The dates on which the to-do item was done
        /// </summary>
        public List<DateOnly> DoneDates { get; set; } = new();
        
    }
}
