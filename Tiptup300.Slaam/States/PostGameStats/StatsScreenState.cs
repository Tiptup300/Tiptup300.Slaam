using System.Collections.Generic;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.States.Match.Actors;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.PostGameStats.StatsBoards;

namespace Tiptup300.Slaam.States.PostGameStats;

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
