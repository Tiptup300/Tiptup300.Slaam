using SlaamMono.Gameplay;
using SlaamMono.Graphing;

namespace SlaamMono.StatsBoards
{
    abstract class StatsBoard
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
