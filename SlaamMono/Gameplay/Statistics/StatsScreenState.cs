using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.StatsBoards;
using System.Collections.Generic;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Gameplay.Statistics
{
    public class StatsScreenState : IState
    {
        public MatchScoreCollection ScoreCollection;
        public GameType GameType;
        public IntRange CurrentPage = new IntRange(0, 0, 2);
        public IntRange CurrentChar;
        public StatsBoard PlayerStats;
        public StatsBoard Kills;
        public StatsBoard PvP;
        public CachedTexture[] _statsButtons = new CachedTexture[3];
        public List<CharacterActor> Characters { get; set; } = new List<CharacterActor>();
    }
}
