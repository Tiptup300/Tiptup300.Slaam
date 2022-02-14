using ZzziveGameEngine;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenRequest : IRequest
    {
        public MatchScoreCollection ScoreCollection;

        public StatsScreenRequest(MatchScoreCollection scoreCollection)
        {
            ScoreCollection = scoreCollection;
        }
    }
}
