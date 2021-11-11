using SlaamMono.Composition.x_;
using SlaamMono.Library.Logging;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using System;
using System.IO;

namespace SlaamMono.Gameplay
{
    public class CurrentMatchSettings
    {

        public static int LivesAmt;
        public static int KillsToWin;
        public static GameType GameType;
        public static float SpeedMultiplyer;
        public static TimeSpan TimeOfMatch;
        public static TimeSpan RespawnTime;
        public static string BoardLocation;

        public static void SaveValues(LobbyScreen parent, string boardloc)
        {
            switch (parent.MainMenu.Items[0].ToSetting().OptionChoice.Value)
            {
                case 0:
                    GameType = GameType.Classic;
                    break;
                case 1:
                    GameType = GameType.Spree;
                    break;
                case 2:
                    GameType = GameType.TimedSpree;
                    break;
            }
            switch (parent.MainMenu.Items[1].ToSetting().OptionChoice.Value)
            {
                case 0:
                    LivesAmt = 3;
                    break;
                case 1:
                    LivesAmt = 5;
                    break;
                case 2:
                    LivesAmt = 10;
                    break;
                case 3:
                    LivesAmt = 20;
                    break;
                case 4:
                    LivesAmt = 40;
                    break;
                case 5:
                    LivesAmt = 50;
                    break;
                case 6:
                    LivesAmt = 100;
                    break;
            }

            switch (parent.MainMenu.Items[2].ToSetting().OptionChoice.Value)
            {
                case 0:
                    SpeedMultiplyer = .5f;
                    break;
                case 1:
                    SpeedMultiplyer = .75f;
                    break;
                case 2:
                    SpeedMultiplyer = 1f;
                    break;
                case 3:
                    SpeedMultiplyer = 1.25f;
                    break;
                case 4:
                    SpeedMultiplyer = 1.5f;
                    break;
            }

            switch (parent.MainMenu.Items[3].ToSetting().OptionChoice.Value)
            {
                case 0:
                    TimeOfMatch = new TimeSpan(0, 1, 0);
                    break;
                case 1:
                    TimeOfMatch = new TimeSpan(0, 2, 0);
                    break;
                case 2:
                    TimeOfMatch = new TimeSpan(0, 3, 0);
                    break;
                case 3:
                    TimeOfMatch = new TimeSpan(0, 5, 0);
                    break;
                case 4:
                    TimeOfMatch = new TimeSpan(0, 10, 0);
                    break;
                case 5:
                    TimeOfMatch = new TimeSpan(0, 20, 0);
                    break;
                case 6:
                    TimeOfMatch = new TimeSpan(0, 40, 0);
                    break;
                case 7:
                    TimeOfMatch = new TimeSpan(0, 45, 0);
                    break;
                case 8:
                    TimeOfMatch = new TimeSpan(0, 60, 0);
                    break;
            }

            switch (parent.MainMenu.Items[4].ToSetting().OptionChoice.Value)
            {
                case 0:
                    RespawnTime = new TimeSpan(0, 0, 0);
                    break;
                case 1:
                    RespawnTime = new TimeSpan(0, 0, 2);
                    break;
                case 2:
                    RespawnTime = new TimeSpan(0, 0, 4);
                    break;
                case 3:
                    RespawnTime = new TimeSpan(0, 0, 6);
                    break;
                case 4:
                    RespawnTime = new TimeSpan(0, 0, 8);
                    break;
                case 5:
                    RespawnTime = new TimeSpan(0, 0, 10);
                    break;
            }

            switch (parent.MainMenu.Items[5].ToSetting().OptionChoice.Value)
            {
                case 0:
                    KillsToWin = 5;
                    break;
                case 1:
                    KillsToWin = 10;
                    break;
                case 2:
                    KillsToWin = 15;
                    break;
                case 3:
                    KillsToWin = 20;
                    break;
                case 4:
                    KillsToWin = 25;
                    break;
                case 5:
                    KillsToWin = 30;
                    break;
                case 6:
                    KillsToWin = 40;
                    break;
                case 7:
                    KillsToWin = 50;
                    break;
                case 8:
                    KillsToWin = 100;
                    break;

            }

            BoardLocation = boardloc;

            XnaContentWriter writer = new XnaContentWriter(x_Di.Get<ProfileFileVersion>());
            writer.Initialize(DialogStrings.MatchSettingsFilename);

            for (int x = 0; x < 6; x++)
            {
                int y = parent.MainMenu.Items[x].ToSetting().OptionChoice.Value;
                writer.Write(y);
            }
            writer.Write(BoardLocation != null ? BoardLocation : "");

            writer.Close();
        }

        private static XnaContentReader reader;
        private static ILogger _logger;

        public static void Initialize(ILogger logger)
        {
            _logger = logger;
        }

        public static void ReadValues(LobbyScreen parent)
        {
            try
            {

                reader = new XnaContentReader(_logger, x_Di.Get<ProfileFileVersion>());
                reader.Initialize(DialogStrings.MatchSettingsFilename);

                if (reader.IsWrongVersion())
                {
                    SaveValues(parent, BoardLocation);
                    throw new EndOfStreamException();
                }

                for (int x = 0; x < 6; x++)
                {
                    int y = reader.ReadInt32();
                    parent.MainMenu.Items[x].ToSetting().ChangeValue(y);
                }

                BoardLocation = reader.ReadString();
            }
            catch (EndOfStreamException)
            {
            }
            finally
            {
                reader.Close();
                SaveValues(parent, BoardLocation);
            }
        }
    }
}
