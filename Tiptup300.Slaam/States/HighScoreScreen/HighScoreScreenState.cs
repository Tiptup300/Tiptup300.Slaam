using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.States.PostGameStats.StatsBoards;

namespace Tiptup300.Slaam.States.HighScoreScreen;

public class HighScoreScreenState : IState
{
   public SurvivalStatsBoard _statsboard { get; set; }
}
