using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library.Graphing;

namespace SlaamMono.States.PostGameStats.StatsBoards;

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
