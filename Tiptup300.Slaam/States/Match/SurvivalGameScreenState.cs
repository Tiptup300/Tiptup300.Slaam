using ZzziveGameEngine.StateManagement;
using Timer = SlaamMono.SubClasses.Timer;

namespace SlaamMono.Survival;

public class SurvivalGameScreenState : IState
{
   public SubClasses.Timer TimeToAddBot = new Timer(new TimeSpan(0, 0, 10));
   public int BotsToAdd = 1;
   public int BotsAdded = 0;
}
