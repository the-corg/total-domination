using TotalDomination.Properties;

namespace TotalDomination.Utilities
{
    /// <summary>
    /// Performs calculations, e.g., for color interpolation 
    /// </summary>
    public class Calculations
    {

        #region Private fields 
        // delta x for the color interpolation
        double _delta;
        // the sum of frequencies of all to-do items
        int _totalFrequency;

        #endregion


        #region Properties with shared data

        /// <summary>
        /// The sum of frequencies of all to-do items
        /// </summary>
        public int TotalFrequency
        {
            get => _totalFrequency;
            set
            {
                if (value == 0)
                {
                    _totalFrequency = 1;
                }
                _totalFrequency = value;
                _delta = 2.0 / _totalFrequency;
            }
        }
        #endregion


        #region Public methods

        /// <summary>
        /// Calculates the interpolated value for a color component 
        /// based on the number of days since the to-do item was last done.
        /// Uses a quadratic curve to approximate color change
        /// (using ScRGB moved away from green and red too quickly, and conversely,
        /// linearly changing normal sRGB stayed with red and green far too long)
        /// </summary>
        /// <param name="daysSinceDone">Number of days since the to-do item was last done</param>
        /// <returns>Value for one color component</returns>
        public byte InterpolatedColorValue(int daysSinceDone)
        {
            if (daysSinceDone <= 0)
                return 0;

            int days = daysSinceDone;

            if (days > TotalFrequency)
            {
                // Urgency tier 1
                days = 2 * TotalFrequency - daysSinceDone + 2;
            }

            // -(x-3)^2 + 9, where x changes from 0 to 2, hence 2.0 in _delta
            var xminus3 = (days - 1) * _delta - 3;
            var quadraticCurveValue = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;

            return (byte)quadraticCurveValue;
        }

        /// <summary>
        /// Calculates the "urgency tier" based on the number of days 
        /// since the to-do item was last done
        /// </summary>
        /// <param name="daysSinceDone">Number of days since the to-do item was last done</param>
        /// <returns>The urgency tier (0 means not urgent at all)</returns>
        public int UrgencyTier(int daysSinceDone)
        {
            if (TotalFrequency == 0)
                return -1;

            return (daysSinceDone - 1) / TotalFrequency;
        }
        #endregion


        #region Static helper methods

        /// <summary>
        /// Returns today's date shifted by NewDayStart hours.<br/>
        /// E.g., for NewDayStart = 4, new day starts at 4:00 o'clock.<br/>
        /// Thus, for time from 0:00 to 4:00, the previous day will be returned
        /// </summary>
        /// <returns>Today's date taking into account midnight shift</returns>
        public static DateOnly GetTodayWithMidnightShift()
        {
            var currentTime = DateTime.Now;
            var today = DateOnly.FromDateTime(currentTime);

            if (currentTime.Hour < Settings.Default.NewDayStart)
            {
                today.AddDays(-1);
            }

            return today;
        }
        #endregion
    }
}
