using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenRequest
    {
        public MatchScoreCollection ScoreCollection;

        public StatsScreenRequest(MatchScoreCollection scoreCollection)
        {
            ScoreCollection = scoreCollection;
        }
    }
}
