using Microsoft.Xna.Framework;
using System;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Gameplay;

 public class MatchTimerState : IState
 {
     public TimeSpan CurrentGameTime = new TimeSpan();
     public bool Moving = false;

     public TimeSpan GameMatchTime;
     public TimeSpan TimeRemaining;
     public Vector2 Position;
     public float MovementSpeed = 10f / 10f;
     public GameType GameType;
     public float StepSize;
     public float CurrentStep;
     public TimeSpan EndingTime;

 }
