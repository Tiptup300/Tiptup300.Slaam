using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.StatsBoards;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenState : IState
    {
        public MatchScoreCollection _scoreCollection;
        public IntRange CurrentPage = new IntRange(0, 0, 2);
        public IntRange CurrentChar;
        public StatsBoard PlayerStats;
        public StatsBoard Kills;
        public StatsBoard PvP;
        public CachedTexture[] _statsButtons = new CachedTexture[3];
    }
}
