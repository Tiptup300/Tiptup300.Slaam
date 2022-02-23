using ZzziveGameEngine;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenRequest : IRequest
    {
        public MatchScoreCollection ScoreCollection;
        public GameType GameType;

        public StatsScreenRequest(MatchScoreCollection scoreCollection, GameType gameType)
        {
            ScoreCollection = scoreCollection;
            GameType = gameType;
        }
    }
}
