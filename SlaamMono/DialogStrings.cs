using SlaamMono.Gameplay;
using SlaamMono.x_;

namespace SlaamMono
{
    /// <summary>
    /// Collection of strings for nearly all dialog of Slaam!
    /// </summary>
    static class DialogStrings
    {

        public const string ContinueSelected = "> Continue <";
        public const string Continue = "Continue";
        public const string QuitSelected = "> Quit <";
        public const string Quit = "Quit";

        public const string SpeedUpName = "Speed UP!";
        public const string SpeedDoownName = "Speed Doown";
        public const string InversionName = "Inversion";

        // Storage
        public const string StorageLocation = "Slaam! Saved Files";
        public const string ProfileFilename = "profiles.pro";
        public const string SurvivalScoresFilename = "survivalscores.g2g";
        public const string MatchSettingsFilename = "settings.cya";

        // Main Menu
        public const string Start = "Start";
        public const string LocalGame = "Local Match";
        public const string OnlinePlay = "Online Play";
        public const string Survival = "Survival";
        public const string SearchForGames = "Search For Games";
        public const string JoinAGame = "Join a Game";
        public const string HostAGame = "Host a Game";
        public const string ManageProfiles = "Manage Profiles";
        public const string Options = "Options";
        public const string Credits = "Credits";
        public const string ViewHighScores = "High Score List";

        // Lobby
        public const string CurrentPeers = "Current Peers: ";
        public const string CurrentBoard = "Board: ";
        public const string CreatedBy = "Created by ";
        public const string Player = "Player ";

        public static string GetDescMsg()
        {
            if (CurrentMatchSettings.GameType == GameType.Classic)
            {
                return "Mode: Classic\n" +
                "Board: " + CleanMapName(CurrentMatchSettings.BoardLocation) + "\n" +
                CurrentMatchSettings.LivesAmt + " Lives\n" +
                CurrentMatchSettings.SpeedMultiplyer + "x Speed\n" +
                CurrentMatchSettings.RespawnTime.TotalSeconds + " Second Respawn";
            }
            else if (CurrentMatchSettings.GameType == GameType.TimedSpree)
            {
                return "Mode: Timed Spree\n" +
                "Board: " + CleanMapName(CurrentMatchSettings.BoardLocation) + "\n" +
                CurrentMatchSettings.TimeOfMatch.TotalMinutes + " Minutes Long\n" +
                CurrentMatchSettings.SpeedMultiplyer + "x Speed\n" +
                CurrentMatchSettings.RespawnTime.TotalSeconds + " Second Respawn";
            }
            else
            {
                return "Mode: Spree\n" +
                "Board: " + CleanMapName(CurrentMatchSettings.BoardLocation) + "\n" +
                CurrentMatchSettings.KillsToWin + " Kills To Win\n" +
                CurrentMatchSettings.SpeedMultiplyer + "x Speed\n" +
                CurrentMatchSettings.RespawnTime.TotalSeconds + " Second Respawn";
            }
        }

        public static string CleanMapName(string str)
        {
            if (str == null)
                return "";
            string[] strs = new string[2];
            strs[0] = str.Substring(str.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\" + GameGlobals.TEXTURE_FILE_PATH, "");
            strs[1] = ((str.IndexOf('_') >= 0) ? str.Substring(0, str.IndexOf('_')).Replace(".png", "").Replace("boards\\" + GameGlobals.TEXTURE_FILE_PATH, "") : "");
            if (strs[1] != "" && strs[1] != "0")
            {
                return strs[0] + " by " + strs[1];
            }
            else
                return strs[0];
        }

        // Char Select
        public const string PressStartToJoin = "Press start to join.";
        public const string SelectAProfile = "Select a profile.";
        public const string PlayingAs = "Playing as ";

        // Stats
        public const string Played = "Played ";
        public const string BestTime = "Best Time: ";
        public const string Used = "Used ";
        public const string Died = "Died ";
        public const string Games = " Games";
        public const string Times = " Times";
        public const string Kills = " Kills";
        public const string Powerups = " Powerups";

        // Feeds
        public const string MenuScreenFeed = "Welcome to the main menu! Please use the directional keys, ctrl, and shift to navigate via keyboard. READ THE README!";
        public const string LobbyScreenFeed = "Welcome to the lobby. Use the up/down controls to add/remove bots. Choose a map then press the Action button when finished.";
        public const string HostingScreenFeed = "Welcome to the hosting screen. Players may automatically connect to you now. If you need the ip of your computer refer to http://whatsmyip.com/ for retreiving it.";
        public const string CharacterSelectScreenFeed = "Welcome to character select. Where you get to choose your profile and character for use in game matches. If you would like to create a profile, please use the Profile Manager in the options menu.";
        public const string BoardSelectScreenFeed = "Welcome to the board design select screen. Where you get to choose the desired design of flooring for the gamematch.";
    }
}
