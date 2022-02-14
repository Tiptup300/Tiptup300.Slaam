using Microsoft.Xna.Framework;
using SlaamMono.SubClasses;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay
{
    public class GameScreenState : IState
    {
        public bool Timing { get; set; } = false;
        public bool IsPaused { get; set; } = false;
        public int KillsToWin { get; set; } = 0;
        public Timer PowerupTime { get; set; }
        public float SpreeStepSize { get; set; }
        public float SpreeCurrentStep { get; set; }
        public int SpreeHighestKillCount { get; set; }
        public int StepsRemaining { get; set; }
        public Vector2 Boardpos { get; set; }
        public int BoardSize { get; set; } = 0;
    }
}
