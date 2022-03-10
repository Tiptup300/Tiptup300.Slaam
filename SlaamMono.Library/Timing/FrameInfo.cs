using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Library.Timing
{
    public struct FrameInfo
    {
        public float MovementFactor { private set; get; }
        public TimeSpan MovementFactorTimeSpan { get { return new TimeSpan(0, 0, 0, 0, (int)MovementFactor); } }
        public int FDPS { private set; get; }
        public int FUPS { private set; get; }
    }
}
