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

        // target number of to-do items per day
        private readonly int _todosPerDay = Settings.Default.TodosPerDay;

        private Todo _model;
        private bool _isDone;
        private double _averageDaysPreviously;
        private double _averageDaysIfDoneToday;
        private int _maximumDaysPreviously;
        private int _maximumDaysIfDoneToday;
        private double _medianDaysPreviously;
        private double _medianDaysIfDoneToday;
        private bool _showIntervalStats;
        private IEnumerable<KeyValuePair<int, int>> _intervalsSortedByFrequencies = [];

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
        public string Info => BuildInfoString();

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
                int urgencyTier = _calculations.UrgencyTier(Urgency);

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
        /// The urgency of the to-do item
        /// </summary>
        public int Urgency => DaysSinceDone * Frequency * _todosPerDay;

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
                CalculateDayStats();
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
                int urgencyTier = _calculations.UrgencyTier(Urgency);
                Color color;

                if (urgencyTier == -1)
                    color = new Color() { A = 255, R = 32, G = 32, B = 32 };
                else if (urgencyTier == 0)
                    color = new Color() { A = 255, R = _calculations.InterpolatedColorValue(Urgency), G = 255, B = 0 };
                else if (urgencyTier == 1)
                    color = new Color() { A = 255, R = 255, G = _calculations.InterpolatedColorValue(Urgency), B = 0 };
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
                int urgencyTier = _calculations.UrgencyTier(Urgency);

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
        /// Calculates _averageDaysPreviously, _averageDaysIfDoneToday,
        /// _medianDaysPreviously, _medianDaysIfDoneToday,
        /// _maximumDaysPreviously, _maximumDaysIfDoneToday,
        /// and the occurrences of intervals
        /// </summary>
        private void CalculateDayStats()
        {
            _showIntervalStats = false;
            _intervalsSortedByFrequencies = [];

            // Done less than two times - no need to show interval stats
            if (DoneDates.Count <= 1)
                return;

            // Regular case
            List<int> intervals = [];

            for (int i = 0; i < DoneDates.Count - 1; i++)
            {
                intervals.Add(DoneDates[i+1].DayNumber - DoneDates[i].DayNumber);
            }

            _intervalsSortedByFrequencies = intervals.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count()).
                OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key);

            var intervalToAdd = Calculations.GetTodayWithMidnightShift().DayNumber - DoneDates.Last().DayNumber;
            if (IsDone)
            {
                intervalToAdd = intervals[^1];
                intervals.RemoveAt(intervals.Count - 1);
            }

            if (intervals.Count == 0)
                return;

            _showIntervalStats = true;

            _maximumDaysPreviously = intervals.Max();
            _averageDaysPreviously = intervals.Average();
            _medianDaysPreviously = Calculations.Median(intervals);

            intervals.Add(intervalToAdd);

            _maximumDaysIfDoneToday = intervals.Max();
            _averageDaysIfDoneToday = intervals.Average();
            _medianDaysIfDoneToday = Calculations.Median(intervals);

        }

        /// <summary>
        /// Creates the string with all info about the to-do item
        /// </summary>
        /// <returns>The string with all info about the to-do item</returns>
        private string BuildInfoString()
        {
            int datesCount = DoneDates.Count;
            double percentage = (Frequency * 100.0) / _calculations.TotalFrequency;
            string result = Title + "\nAdded on " + Added + "\n";
            int urgencyGrowth = Frequency * _todosPerDay;
            int greatestCommonDivisor = Calculations.GreatestCommonDivisor(urgencyGrowth, _calculations.TotalFrequency);
            int realFrequencyNumerator = urgencyGrowth / greatestCommonDivisor;
            int realFrequencyDenominator = _calculations.TotalFrequency / greatestCommonDivisor;

            List<string> s = []; // Collects string parts
            List<int> toEqualize = []; // Collects indices of string parts that need length equalization

            s.Add("\nReal frequency: " + (realFrequencyNumerator == 1 ? "once" : realFrequencyNumerator + " times") + " per " + realFrequencyDenominator + " days");
            s.Add("\nBase frequency: " + Frequency);
            toEqualize.Add(s.Count - 1);
            s.Add(" | " + percentage.ToString("F1") + "% of all tasks");

            s.Add("\nUrgency growth: " + urgencyGrowth + " per day");
            toEqualize.Add(s.Count - 1);
            s.Add(" | at " + _todosPerDay + " tasks per day\n");

            s.Add("\nCurrent urgency: " + Urgency + " / " + _calculations.TotalFrequency);
            toEqualize.Add(s.Count - 1);
            s.Add(" | Tier " + _calculations.UrgencyTier(Urgency));

            if (datesCount > 1 || (datesCount == 1 && !IsDone))
            {
                s.Add("\nPreviously done: " + DaysSinceDone + " day" + (DaysSinceDone != 1 ? "s" : "") + " ago");
                toEqualize.Add(s.Count - 1);
                s.Add(" | " + LastDone + "\n");
            }

            s.Add("\nTimes done: " + datesCount);

            if (_showIntervalStats)
            {
                s.Add("\nMaximum days in between: " + (IsDone ? _maximumDaysIfDoneToday : _maximumDaysPreviously));
                toEqualize.Add(s.Count - 1);
                s.Add(IsDone ? " | Previously: " + _maximumDaysPreviously : " | If done today: " + _maximumDaysIfDoneToday);

                s.Add("\nAverage days in between: " + (IsDone ? _averageDaysIfDoneToday.ToString("F1") : _averageDaysPreviously.ToString("F1")));
                toEqualize.Add(s.Count - 1);
                s.Add(IsDone ? " | Previously: " + _averageDaysPreviously.ToString("F1") : " | If done today: " + _averageDaysIfDoneToday.ToString("F1"));

                s.Add("\nMedian days in between: " + (IsDone ? _medianDaysIfDoneToday.ToString("F1") : _medianDaysPreviously.ToString("F1")));
                toEqualize.Add(s.Count - 1);
                s.Add(IsDone ? " | Previously: " + _medianDaysPreviously.ToString("F1") : " | If done today: " + _medianDaysIfDoneToday.ToString("F1"));

                s.Add("\n\nDays in between:");
                foreach (var interval in _intervalsSortedByFrequencies)
                {
                    s.Add(" " + interval.Value + "x" + interval.Key + "d");
                }
            }

            // Equalize string lengths
            var maxlength = toEqualize.Select(x => s[x].Length).Max();
            for (int i = 0; i < toEqualize.Count; i++)
                while (s[toEqualize[i]].Length < maxlength)
                    s[toEqualize[i]] += " ";

            result += string.Join("", s);

            if (datesCount > 0)
            {
                result += "\n\nHistory:\n" + string.Join("  ", DoneDates.Select(x => x.ToShortDateString()).Reverse());
            }

            return result;
        }
        #endregion 

    }
}
