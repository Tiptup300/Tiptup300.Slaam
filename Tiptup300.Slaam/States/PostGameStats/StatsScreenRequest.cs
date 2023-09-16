using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenRequestState : IState
    {
        public MatchScoreCollection ScoreCollection;
        public GameType GameType;

        public StatsScreenRequestState(MatchScoreCollection scoreCollection, GameType gameType)
        {
            ScoreCollection = scoreCollection;
            GameType = gameType;
        }
    }
}
