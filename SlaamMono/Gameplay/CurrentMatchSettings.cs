using System;

namespace SlaamMono.Gameplay
{
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

    public static class CurrentMatchSettings
    {
        public static GameType GameType;
        public static int LivesAmt;
        public static float SpeedMultiplyer;
        public static TimeSpan TimeOfMatch;
        public static TimeSpan RespawnTime;
        public static int KillsToWin;
        public static string BoardLocation;

        public static void Set(MatchSettings matchSettings)
        {
            GameType = matchSettings.GameType;
            LivesAmt = matchSettings.LivesAmt;
            SpeedMultiplyer = matchSettings.SpeedMultiplyer;
            TimeOfMatch = matchSettings.TimeOfMatch;
            RespawnTime = matchSettings.RespawnTime;
            KillsToWin = matchSettings.KillsToWin;
            BoardLocation = matchSettings.BoardLocation;
        }
    }
}
