using SlaamMono.SubClasses;
using System;
using ZzziveGameEngine;

namespace SlaamMono.Survival
{
    public class SurvivalGameScreenState : IState
    {
        public Timer TimeToAddBot = new Timer(new TimeSpan(0, 0, 10));
        public int BotsToAdd = 1;
        public int BotsAdded = 0;
    }
}
