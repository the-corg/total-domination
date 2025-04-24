namespace TotalDomination.Model
{
    /// <summary>
    /// Model class for a To do item
    /// </summary>
    public class Todo
    {
        public required string Title { get; set; }
        public int Frequency { get; set; } = 1;
        public DateOnly? LastDone { get; set; }

    }
}
