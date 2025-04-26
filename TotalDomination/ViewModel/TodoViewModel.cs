using System.Windows;
using System.Windows.Media;
using TotalDomination.Model;
using TotalDomination.Properties;
using TotalDomination.Utilities;

namespace TotalDomination.ViewModel
{
    /// <summary>
    /// View model for a single To-do item
    /// </summary>
    public class TodoViewModel : BaseViewModel
    {
        #region Private fields and the constructor

        private readonly Calculations _calculations;

        private Todo _model;
        private bool _isDone;
        private double _averageDaysPreviously = -1;
        private double _averageDaysIfDoneToday = -1;
        private int _maximumDaysPreviously = -1;
        private int _maximumDaysIfDoneToday = -1;

        public TodoViewModel(Todo model, Calculations calculations)
        {
            _model = model;
            _calculations = calculations;

            if (_model.DoneDates.Count > 0)
                _isDone = Calculations.GetTodayWithMidnightShift() == _model.DoneDates.Last();

            CalculateDayStats();
        }
        #endregion

        #region Public properties 

        /// <summary>
        /// All info about the to-do item 
        /// </summary>
        public string Info
        {
            get
            {
                int datesCount = DoneDates.Count;
                int todosPerDay = Settings.Default.TodosPerDay;
                double percentage = (Frequency * 100.0) / _calculations.TotalFrequency;
                string result = Title + "\nAdded on " + Added + "\n";

                List<string> s = []; // Collects string parts
                List<int> toEqualize = []; // Collects indices of string parts that need length equalization
                s.Add("\nFrequency: " + Frequency * todosPerDay + "/" + _calculations.TotalFrequency + " at " + todosPerDay + " tasks/day");
                toEqualize.Add(s.Count - 1);
                s.Add(" | " + percentage.ToString("F1") + "% of all tasks\n");

                if (datesCount > 1 || (datesCount == 1 && !IsDone))
                {
                    s.Add("\nPreviously done: " + DaysSinceDone + " day" + (DaysSinceDone != 1 ? "s" : "") + " ago");
                    toEqualize.Add(s.Count - 1);
                    s.Add(" | " + LastDone + "\n");
                }

                s.Add("\nTimes done previously: " + (IsDone ? datesCount - 1 : datesCount));

                if (IsDone)
                {
                    if (_averageDaysIfDoneToday >= 0)
                    {
                        s.Add("\nAverage days in between: " + _averageDaysIfDoneToday.ToString("F1"));
                        toEqualize.Add(s.Count - 1);
                    }
                    if (_averageDaysPreviously >= 0)
                        s.Add(" | Previously: " + _averageDaysPreviously.ToString("F1"));
                    if (_maximumDaysIfDoneToday >= 0)
                    {
                        s.Add("\nMaximum days in between: " + _maximumDaysIfDoneToday);
                        toEqualize.Add(s.Count - 1);
                    }
                    if (_maximumDaysPreviously >= 0)
                        s.Add(" | Previously: " + _maximumDaysPreviously);
                }
                else
                {
                    if (_averageDaysPreviously >= 0)
                    {
                        s.Add("\nAverage days in between: " + _averageDaysPreviously.ToString("F1"));
                        toEqualize.Add(s.Count - 1);
                        s.Add(" | If done today: " + _averageDaysIfDoneToday.ToString("F1"));
                    }
                    if (_maximumDaysPreviously >= 0)
                    {
                        s.Add("\nMaximum days in between: " + _maximumDaysPreviously);
                        toEqualize.Add(s.Count - 1);
                        s.Add(" | If done today: " + _maximumDaysIfDoneToday);
                    }
                }

                // Equalize string lengths
                var maxlength = toEqualize.Select(x => s[x].Length).Max();
                for (int i = 0; i < toEqualize.Count; i++) 
                    while (s[toEqualize[i]].Length < maxlength) 
                        s[toEqualize[i]] += " ";

                result += string.Join("", s);

                if (datesCount > 1 || (datesCount == 1 && !IsDone))
                {
                    result += "\n\nHistory:\n" + string.Join("  ", DoneDates.Select(x => x.ToShortDateString()));
                }

                // TODO: remove! (debug code)
                // result += "\n\nTier: " + _calculations.UrgencyTier(DaysSinceDone);
                // result += "\nColor: " + ColorBrush.Color.R + " " + ColorBrush.Color.G + " " + ColorBrush.Color.B;

                return result;
            }
        }

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
                OnPropertyChanged(nameof(Info));
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

        #region Helper methods 

        /// <summary>
        /// Calculates _averageDaysPreviously, _averageDaysIfDoneToday, _maximumDaysPreviously, and _maximumDaysIfDoneToday
        /// </summary>
        private void CalculateDayStats()
        {
            // Edge cases
            if (DoneDates.Count == 0 || (DoneDates.Count == 1 && IsDone))
                return;
            if (DoneDates.Count == 1) // IsDone is false here
            {
                _maximumDaysIfDoneToday = Calculations.GetTodayWithMidnightShift().DayNumber - DoneDates[0].DayNumber;
                _averageDaysIfDoneToday = _maximumDaysIfDoneToday;
                return;
            }

            // Regular case
            List<int> intervals = [];

            for (int i = 0; i < DoneDates.Count - 1; i++)
            {
                intervals.Add(DoneDates[i+1].DayNumber - DoneDates[i].DayNumber);
            }
            
            if (IsDone)
            {
                _maximumDaysIfDoneToday = intervals.Max();
                _averageDaysIfDoneToday = intervals.Average();
                if (intervals.Count > 1)
                {
                    intervals.RemoveAt(intervals.Count - 1);
                    _maximumDaysPreviously = intervals.Max();
                    _averageDaysPreviously = intervals.Average();
                }
            }
            else
            {
                _maximumDaysPreviously = intervals.Max();
                _averageDaysPreviously = intervals.Average();
                intervals.Add(Calculations.GetTodayWithMidnightShift().DayNumber - DoneDates.Last().DayNumber);
                _maximumDaysIfDoneToday = intervals.Max();
                _averageDaysIfDoneToday = intervals.Average();
            }
        }
        #endregion 

    }
}
