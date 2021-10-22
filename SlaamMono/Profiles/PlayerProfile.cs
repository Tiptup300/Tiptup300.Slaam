using System;

namespace SlaamMono.Profiles
{
    public class PlayerProfile
    {
        public int TotalKills = 0;
        public int TotalDeaths = 0;
        public int TotalPowerups = 0;
        public int TotalGames = 0;
        public string Name;
        public string Skin = "";
        public bool IsBot = false;
        public bool Used = false;
        public TimeSpan BestGame = TimeSpan.Zero;

        public PlayerProfile(int totalkills, int totalgames, int totaldeaths, string skin, string name,
                             bool isbot, int totalpowerups, int bestgame)
        {
            TotalKills = totalkills;
            TotalGames = totalgames;
            TotalDeaths = totaldeaths;
            Skin = skin;
            Name = name;
            IsBot = isbot;
            TotalPowerups = totalpowerups;
            BestGame = new TimeSpan(0, 0, 0, 0, bestgame);
        }

        public PlayerProfile(string name, bool isbot)
        {
            Name = name;
            IsBot = isbot;
        }
    }
}
