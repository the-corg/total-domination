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
        private double _delta;
        // the sum of frequencies of all to-do items
        private int _totalFrequency;

        #endregion


        #region Public properties 

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
        /// based on the "urgency" of a to-do item.
        /// Uses a quadratic curve to approximate color change
        /// (using ScRGB moved away from green and red too quickly, and conversely,
        /// linearly changing normal sRGB stayed with red and green far too long)
        /// </summary>
        /// <param name="urgency">Urgency of the to-do item</param>
        /// <returns>Value for one color component</returns>
        public byte InterpolatedColorValue(int urgency)
        {
            if (urgency <= 0)
                return 0;

            if (urgency > TotalFrequency)
            {
                // Urgency tier 1
                // The green color component should be decreasing here
                // along the same curve that was used to calculate the growth of
                // the red color component for urgency tier 0
                urgency = 2 * TotalFrequency - urgency + 2;
            }

            // -(x-3)^2 + 9, where x changes from 0 to 2, hence 2.0 in _delta
            var xminus3 = (urgency - 1) * _delta - 3;
            var quadraticCurveValue = 255.0 * (9.0 - xminus3 * xminus3) / 8.0;

            return (byte)quadraticCurveValue;
        }

        /// <summary>
        /// Calculates the "urgency tier"
        /// </summary>
        /// <param name="urgency">Urgency of the to-do item</param>
        /// <returns>The urgency tier (0 means not urgent at all)</returns>
        public int UrgencyTier(int urgency)
        {
            if (TotalFrequency == 0)
                return -1;

            return (urgency - 1) / TotalFrequency;
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
                return today.AddDays(-1);
            }

            return today;
        }

        /// <summary>
        /// Recursively computes the greatest common divisor of two numbers
        /// using the Euclidean algorithm
        /// </summary>
        /// <returns>The greatest common divisor of <paramref name="a"/> and <paramref name="b"/></returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? Math.Abs(a) : GreatestCommonDivisor(b, a % b);
        }
        #endregion
    }
}
