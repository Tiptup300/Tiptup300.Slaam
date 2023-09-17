using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.PostGameStats.StatsBoards;

public abstract class StatsBoard
{
   public Graph MainBoard;
   public MatchScoreCollection ParentScoreCollector;
   protected readonly StatsScreenState _statsScreenState;

   public StatsBoard(MatchScoreCollection parentscorecollector, StatsScreenState statsScreenState)
   {
      ParentScoreCollector = parentscorecollector;
      _statsScreenState = statsScreenState;
   }

   public abstract void CalculateStats();

   public abstract Graph ConstructGraph(int index);
}
