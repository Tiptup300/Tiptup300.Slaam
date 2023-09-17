using System;

namespace SlaamMono.Gameplay;

 public class MatchSettings
 {
     public GameType GameType { get; set; }
     public int LivesAmt { get; set; }
     public float SpeedMultiplyer { get; set; }
     public TimeSpan TimeOfMatch { get; set; }
     public TimeSpan RespawnTime { get; set; }
     public int KillsToWin { get; set; }
     public string BoardLocation { get; set; }
     public int[] SettingsIndexes { get; set; }
 }
