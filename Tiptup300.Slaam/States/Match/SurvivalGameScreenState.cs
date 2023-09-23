using System.Tiptup300.StateManagement;

namespace Tiptup300.Slaam.States.Match;

public class SurvivalGameScreenState : IState
{
   public Library.Widgets.TimerWidget TimeToAddBot = new Library.Widgets.TimerWidget(new TimeSpan(0, 0, 10));
   public int BotsToAdd = 1;
   public int BotsAdded = 0;
}
