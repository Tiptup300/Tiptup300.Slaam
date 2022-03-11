using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Library.Timing
{
    public struct FrameTimeServiceState
    {
        public Frame _latestFrame;
        public int _framesDrawn;
        public int _framesDrawnLast;
        public int _framesUpdated;
        public int _framesUpdatedLast;
        public TimeSpan _currentTimer;
    }
}
