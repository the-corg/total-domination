using System.Windows;
using System.Windows.Media;
using TotalDomination.Model;
using TotalDomination.Utilities;

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

            if (_model.DoneDates.Count > 0)
                _isDone = Calculations.GetTodayWithMidnightShift() == _model.DoneDates.Last();
        }
        #endregion

        #region Public properties 

        /// <summary>
        /// Title of the To-do item
        /// </summary>
        public string Title => _model.Title;

        /// <summary>
        /// Fires in front of the To-do item (when it's long overdue) 
        /// </summary>
        public string Fires
        {
            get
            {
                string fires = "";
                int urgencyTier = _calculations.UrgencyTier(DaysSinceDone);

                for (int i = 0; i < urgencyTier - 1; i++)
                    fires += "🔥";

                if (fires.Length > 0)
                    fires += " ";

                return fires;
            }
        }

        /// <summary>
        /// Frequency of the To-do item
        /// </summary>
        public int Frequency => _model.Frequency;

        /// <summary>
        /// The date when the to-do item was first loaded from a To-do list file
        /// </summary>
        public DateOnly Added => _model.Added;

        /// <summary>
        /// The dates on which the to-do item was done
        /// </summary>
        public List<DateOnly> DoneDates => _model.DoneDates;

        /// <summary>
        /// The date when the To-do item was last marked as done (or day of adding, if never)
        /// </summary>
        public DateOnly LastDone
        {
            get
            {
                var count = _model.DoneDates.Count;
                if (count > 0)
                {
                    if (IsDone)
                    {
                        if (count > 1)
                            return _model.DoneDates[count - 2];
                    }
                    else
                    {
                        return _model.DoneDates.Last();
                    }
                }

                return _model.Added;
            }
        }

        /// <summary>
        /// Number of days since the to-do item was last done (0 = today, 1 = yesterday)
        /// </summary>
        public int DaysSinceDone => Calculations.GetTodayWithMidnightShift().DayNumber - LastDone.DayNumber;

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

                if (value)
                {
                    // CheckBox was selected
                    _model.DoneDates.Add(Calculations.GetTodayWithMidnightShift());
                }
                else
                {
                    // CheckBox was unselected
                    int count = _model.DoneDates.Count;
                    if (count > 0)
                    {
                        _model.DoneDates.RemoveAt(count - 1);
                    }
                }

                // TODO : Call OnPropertyChanged? or just sort the list? sort using ListCollectionView?
            }
        }

        /// <summary>
        /// Brush with the color of the To-do item (based on urgency)
        /// </summary>
        public SolidColorBrush ColorBrush
        {
            get
            {
                int urgencyTier = _calculations.UrgencyTier(DaysSinceDone);
                Color color;

                if (urgencyTier == -1)
                    color = new Color() { A = 255, R = 32, G = 32, B = 32 };
                else if (urgencyTier == 0)
                    color = new Color() { A = 255, R = _calculations.InterpolatedColorValue(DaysSinceDone), G = 255, B = 0 };
                else if (urgencyTier == 1)
                    color = new Color() { A = 255, R = 255, G = _calculations.InterpolatedColorValue(DaysSinceDone), B = 0 };
                else
                    color = new Color() { A = 255, R = 255, G = 0, B = 0 };

                return new SolidColorBrush(color);
            }
        }

        /// <summary>
        /// Represents the font weight of the To-do item (based on urgency)
        /// </summary>
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

    }
}
