using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
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
