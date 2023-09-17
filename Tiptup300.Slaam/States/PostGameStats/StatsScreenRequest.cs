using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.PostGameStats;

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
