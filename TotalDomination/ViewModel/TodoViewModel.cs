using System.Windows;
using System.Windows.Media;
using TotalDomination.Data;
using TotalDomination.Model;

namespace TotalDomination.ViewModel
{
    /// <summary>
    /// View model for a single To-do item
    /// </summary>
    public class TodoViewModel : BaseViewModel
    {
        #region Private fields and the constructor

        private Todo _model;
        private bool _isDone;

        private readonly Calculations _calculations;

        public TodoViewModel(Todo model, Calculations calculations)
        {
            _model = model;
            _calculations = calculations;
        }
        #endregion

        #region Public properties 
        /// <summary>
        /// Title of the To-do item
        /// </summary>
        public string Title
        {
            get
            {
                string fires = "";
                int urgencyTier = _calculations.UrgencyTier(DaysSinceDone);
                for (int i = 0; i < urgencyTier - 1; i++)
                {
                    fires += "🔥";
                }
                if (fires.Length > 0)
                {
                    fires += " ";
                }
                return fires + _model.Title + $" Color: R={Color.R} G={Color.G} B={Color.B}";
            }
            set
            {
                if (value == _model.Title)
                    return;

                _model.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Frequency of the To-do item
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
        /// The date when the To-do item was last marked as done
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
        /// Number of days since the to-do item was last done (0 = today, 1 = yesterday)
        /// </summary>
        public int DaysSinceDone { get; set; }

        /// <summary>
        /// Shows whether the To-do item was done today
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

        /// <summary>
        /// Represents the color of the To-do item (based on urgency)
        /// </summary>
        public Color Color
        {
            get
            {
                int urgencyTier = _calculations.UrgencyTier(DaysSinceDone);

                if (urgencyTier == -1)
                    return new Color() { A = 255, R = 32, G = 32, B = 32 };
                else if (urgencyTier == 0)
                    return new Color() { A = 255, R = _calculations.InterpolatedColorValue(DaysSinceDone), G = 255, B = 0 };
                else if (urgencyTier == 1)
                    return new Color() { A = 255, R = 255, G = _calculations.InterpolatedColorValue(DaysSinceDone), B = 0 };
                else
                    return new Color() { A = 255, R = 255, G = 0, B = 0 };
            }
        }


        public FontWeight FontWeight
        {
            get
            {
                int urgencyTier = _calculations.UrgencyTier(DaysSinceDone);

                if (urgencyTier < 3)
                    return FontWeights.Normal;
                else if (urgencyTier == 3)
                    return FontWeights.DemiBold;
                else if (urgencyTier == 4)
                    return FontWeights.Bold;
                else
                    return FontWeights.UltraBold;
            }
        }


        #endregion

        /// <summary>
        /// Calls property changed events for visual data
        /// (that's calculated later than everything else)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /*public void UpdateVisualData()
        {
            throw new NotImplementedException();
        }*/

    }
}
