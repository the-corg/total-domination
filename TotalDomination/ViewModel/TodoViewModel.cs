using TotalDomination.Model;

namespace TotalDomination.ViewModel
{
    /// <summary>
    /// View model for a single To do item
    /// </summary>
    public class TodoViewModel : BaseViewModel
    {
        private Todo _model;
        private bool _isDone;

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
            get => _model.DoneDates.LastOrDefault();
            set
            {
                if (value == _model.DoneDates.LastOrDefault())
                    return;

                if (value is null)
                {
                    if (_model.DoneDates.Count > 0)
                    {
                        _model.DoneDates.RemoveAt(_model.DoneDates.Count - 1);
                    }
                }
                else
                {
                    _model.DoneDates.Add((DateOnly)value);
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Shows whether the To do item was done today
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (value == _isDone)
                    return;

                _isDone = value;
                OnPropertyChanged();
                // TODO : Change LastDone and call OnPropertyChanged too
            }
        }



    }
}
