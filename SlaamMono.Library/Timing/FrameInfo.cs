using System;

namespace SlaamMono.Library.Timing
{
    public struct FrameInfo
    {
        public FrameInfo(DateTime dateTime, float movementFactor, int fDPS, int fUPS)
        {
            DateTime = dateTime;
            MovementFactor = movementFactor;
            FDPS = fDPS;
            FUPS = fUPS;
        }

        public DateTime DateTime { private set; get; }
        public float MovementFactor { private set; get; }
        public TimeSpan MovementFactorTimeSpan { get { return new TimeSpan(0, 0, 0, 0, (int)MovementFactor); } }
        public int FDPS { private set; get; }
        public int FUPS { private set; get; }
    }
}
