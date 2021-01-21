namespace SlaamMono
{
    public class IntRange
    {
        #region Variables

        public int Value = 0;
        public int last = 0;
        public int Min = 0;
        public int Max = 0;

        #endregion

        #region Constructor

        public IntRange(int y)
        {
            Value = y;
        }

        public IntRange(int y, int min, int max)
        {
            Value = y;
            Min = min;
            Max = max;

            if (Min > Max)
                throw new System.Exception("Minimum is greater than maximum, WTF WERE YOU THINKING!?!?");
        }

        #endregion

        #region Preview Methods

        /// <summary>
        /// Previews the previous value.
        /// </summary>
        /// <param name="a">Amount Subtracting</param>
        /// <param name="min">Minimum of Value</param>
        /// <param name="max">Maximum of Value</param>
        /// <returns></returns>
        public int PreviousValue(int a, int min, int max)
        {
            int x = Value;
            Value -= a;
            Check(min, max);

            a = Value;
            Value = x;
            return a;
        }

        /// <summary>
        /// Previews the next value.
        /// </summary>
        /// <param name="a">Amount Adding</param>
        /// <param name="min">Minimum of Value</param>
        /// <param name="max">Maximum of Value</param>
        /// <returns></returns>
        public int NextValue(int a, int min, int max)
        {
            int x = Value;
            Value += a;
            Check(min, max);

            a = Value;
            Value = x;
            return a;
        }

        #endregion

        #region Add Methods

        /// <summary>
        /// Adds the specified amount while staying in the min/max boundries.
        /// </summary>
        /// <param name="a">Amount Adding</param>
        /// <param name="min">Minimum of Value</param>
        /// <param name="max">Maximum of Value</param>
        /// <returns></returns>
        public int Add(int a, int min, int max)
        {
            Value += a;
            Check(min, max);
            last = a;

            return Value;
        }

        /// <summary>
        /// Adds the specified amount while staying in the min/max boundries.
        /// </summary>
        /// <param name="a">Amount Adding</param>
        /// <returns></returns>
        public int Add(int a)
        {
            Value += a;
            Check(Min, Max);
            last = a;

            return Value;
        }

        #endregion

        #region Check Method

        /// <summary>
        /// Decides whether the current value is under/over the current range. If so it resets at the top/bottom and continues.
        /// </summary>
        /// <param name="min">Minimum of Value</param>
        /// <param name="max">Maximum of Value</param>
        public void Check(int min, int max)
        {
            while (Value < min || Value > max)
            {
                if (Value < min)
                {
                    Value = max - ((min - Value) - 1);
                }
                else if (Value > max)
                {
                    Value = min + ((Value - max) - 1);
                }
            }
        }

        #endregion

        #region Sub Methods

        /// <summary>
        /// Subtracts the specified amount while staying in the min/max boundries.
        /// </summary>
        /// <param name="a">Amount Subtracting</param>
        /// <param name="min">Minimum of Value</param>
        /// <param name="max">Maximum of Value</param>
        /// <returns></returns>
        public int Sub(int a, int min, int max)
        {
            Value -= a;
            Check(min, max);
            last = -a;

            return Value;
        }

        /// <summary>
        /// Subtracts the specified amount while staying in the min/max boundries.
        /// </summary>
        /// <param name="a">Amount Subtracting</param>
        /// <returns></returns>
        public int Sub(int a)
        {
            Value -= a;
            Check(Min, Max);
            last = -a;

            return Value;
        }

        #endregion
    }
}
