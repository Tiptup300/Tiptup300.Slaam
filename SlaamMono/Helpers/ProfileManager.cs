#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SlaamMono.Library.Logging;
#endregion

namespace SlaamMono
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public static class ProfileManager
    {
        public static List<PlayerProfile> AllProfiles;
        public static RedirectionList<PlayerProfile> PlayableProfiles;
        public static RedirectionList<PlayerProfile> BotProfiles;
        private static Random rand = new Random();
        private static bool filefound = false;
        public static bool FirstTime = false;

        private static ILogger _logger;

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public static void Initialize(ILogger logger)
        {
            _logger = logger;

            LoadProfiles();
            FirstTime = !filefound;
        }

        public static void LoadProfiles()
        {
            XnaContentReader reader = new XnaContentReader(_logger, DialogStrings.ProfileFilename);

            filefound = !reader.WasNotFound;

            AllProfiles = new List<PlayerProfile>();
            PlayableProfiles = new RedirectionList<PlayerProfile>(AllProfiles);
            BotProfiles = new RedirectionList<PlayerProfile>(AllProfiles);
            AllProfiles.Add(new PlayerProfile(0, 0, 0, "", GameGlobals.DEFAULT_PLAYER_NAME, false, 0, 0));

            PlayableProfiles.Add(0);

            if (reader.IsWrongVersion())
            {
                // Wrong Version, do nothing...
            }
            else
            {
                int ProfileAmt = reader.ReadInt32();
                

                for (int x = 0; x < ProfileAmt; x++)
                {
                    AllProfiles.Add(new PlayerProfile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadString(), reader.ReadString(), reader.ReadBool(), reader.ReadInt32(), reader.ReadInt32()));
                    if (AllProfiles[AllProfiles.Count - 1].IsBot)
                        BotProfiles.Add(AllProfiles.Count - 1);
                    else
                        PlayableProfiles.Add(AllProfiles.Count - 1);
                }

#if ZUNE
                if (PlayableProfiles.Count == 1)
                {
                    AllProfiles.Add(new PlayerProfile(0, 0, 0, "", "tommy", false, 0, 0));
                    PlayableProfiles.Add(AllProfiles.Count - 1);
                }
#endif
            }

            reader.Close();


            if (BotProfiles.Count != Resources.BotNames.Count)
            {
                for (int x = 0; x < AllProfiles.Count; x++)
                {
                    if (AllProfiles[x].IsBot)
                    {
                        AllProfiles.RemoveAt(x);
                        x--;
                    }
                }
                BotProfiles = new RedirectionList<PlayerProfile>(AllProfiles);
                for (int x = 0; x < Resources.BotNames.Count; x++ )
                {
                    AllProfiles.Add(new PlayerProfile(Resources.BotNames[x].Replace("\r",""),true));
                    BotProfiles.Add(AllProfiles.Count - 1);
                }
            }

            SaveProfiles();
        }

        public static void SaveProfiles()
        {
            XnaContentWriter writer = new XnaContentWriter(DialogStrings.ProfileFilename);

            writer.Write(AllProfiles.Count-1);

            for (int x = 1; x < AllProfiles.Count; x++)
            {
                writer.Write(AllProfiles[x].TotalKills);
                writer.Write(AllProfiles[x].TotalGames);
                writer.Write(AllProfiles[x].TotalDeaths);
                writer.Write(AllProfiles[x].Skin);
                writer.Write(AllProfiles[x].Name);
                writer.Write(AllProfiles[x].IsBot);
                writer.Write(AllProfiles[x].TotalPowerups);
                writer.Write((int)AllProfiles[x].BestGame.TotalMilliseconds);
            }

            writer.Close();
        }

        public static void AddNewProfile(PlayerProfile prof)
        {
            AllProfiles.Add(prof);
            if (prof.IsBot)
                BotProfiles.Add(AllProfiles.Count - 1);
            else
                PlayableProfiles.Add(AllProfiles.Count - 1);
        }

        public static int GetBotProfile()
        {
            int index = rand.Next(0,BotProfiles.Count);
            int ct = 0;
            do
            {
                index = rand.Next(0, BotProfiles.Count);
                ct++;

                if (ct > 100000)
                    throw new Exception("Infinite Loop detected...");
            }
            while(BotProfiles[index].Used);

            BotProfiles[index].Used = true;

            return BotProfiles.GetRealIndex(index);
        }

        public static void ResetAllBots()
        {
            for (int x = 0; x < BotProfiles.Count; x++)
                BotProfiles[x].Used = false;
        }

        public static void ResetBot(int index)
        {
            AllProfiles[index].Used = false;
        }

        public static void RemovePlayer(int index)
        {
            AllProfiles.RemoveAt(index);
            SaveProfiles();
            LoadProfiles();
        }
    }

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

        public PlayerProfile(int totalkills,int totalgames,int totaldeaths,string skin,string name,
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


