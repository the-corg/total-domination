using TotalDomination.Model;

namespace TotalDomination.ViewModel
{
    /// <summary>
    /// View model for a single To do item
    /// </summary>
    public class TodoViewModel : BaseViewModel
    {
        private Todo _model;

        public TodoViewModel(Todo model)
        {
            _model = model;
        }

        /// <summary>
        /// Title of the To do item
        /// </summary>
        public string Title
        {
            get => _model.Title;
            set
            {
                if (value == _model.Title)
                    return;

                _model.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Frequency of the To do item
        /// </summary>
        public int Frequency
        {
            get => _model.Frequency;
            set
            {
                if (value == _model.Frequency)
                    return;

                _model.Frequency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The date when the To do item was last marked as done
        /// </summary>
        public DateOnly? LastDone
        {
            get => _model.LastDone;
            set
            {
                if (value == _model.LastDone)
                    return;

                _model.LastDone = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Shows whether the To do item was done today
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// Shows whether the To do item is present in the current todo list
        /// </summary>
        public bool IsActive { get; set; }


    }
}
