using SlaamMono.Gameplay;
using SlaamMono.Library.Graphing;

namespace SlaamMono.StatsBoards
{
    public abstract class StatsBoard
    {
        public Graph MainBoard;
        public MatchScoreCollection ParentScoreCollector;

        public StatsBoard(MatchScoreCollection parentscorecollector)
        {
            ParentScoreCollector = parentscorecollector;
        }

        public abstract void CalculateStats();

        public abstract Graph ConstructGraph(int index);
    }
}
