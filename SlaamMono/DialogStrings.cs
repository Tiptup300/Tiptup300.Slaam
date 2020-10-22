using System;
using System.Collections.Generic;
using System.Text;

namespace Slaam
{
    /// <summary>
    /// Collection of strings for nearly all dialog of Slaam!
    /// </summary>
    static class DialogStrings
    {

        #region GameScreen

        public const string ContinueSelected = "> Continue <";
        public const string Continue = "Continue";
        public const string QuitSelected = "> Quit <";
        public const string Quit = "Quit";

        #endregion

        #region Powerups

        public const string SpeedUpName = "Speed UP!";
        public const string SpeedDoownName = "Speed Doown";
        public const string InversionName = "Inversion";

        #endregion

        #region Storage

        // Storage
        public const string StorageLocation = "Slaam! Saved Files";
        public const string ProfileFilename = "profiles.pro";
        public const string SurvivalScoresFilename = "survivalscores.g2g";
        public const string MatchSettingsFilename = "settings.cya";

        #endregion

        #region Main Menu

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

        #endregion

        #region Lobby

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
            if(strs[1] != "" && strs[1] != "0")
            {
                return strs[0] + " by " + strs[1];
            }
            else
                return strs[0];
        }

        #endregion

        #region Char Select

        // Char Select
        public const string PressStartToJoin = "Press start to join.";
        public const string SelectAProfile = "Select a profile.";
        public const string PlayingAs = "Playing as ";

        #endregion

        #region Stats

        // Stats
        public const string Played = "Played ";
        public const string BestTime = "Best Time: ";
        public const string Used = "Used ";
        public const string Died = "Died ";
        public const string Games = " Games";
        public const string Times = " Times";
        public const string Kills = " Kills";
        public const string Powerups = " Powerups";

        #endregion

        #region Feeds

        // Feeds
        public const string MenuScreenFeed = "Welcome to the main menu! Please use the directional keys, ctrl, and shift to navigate via keyboard. READ THE README!";
        public const string LobbyScreenFeed = "Welcome to the lobby. Use the up/down controls to add/remove bots. Choose a map then press the Action button when finished.";
        public const string HostingScreenFeed = "Welcome to the hosting screen. Players may automatically connect to you now. If you need the ip of your computer refer to http://whatsmyip.com/ for retreiving it.";
        public const string CharacterSelectScreenFeed = "Welcome to character select. Where you get to choose your profile and character for use in game matches. If you would like to create a profile, please use the Profile Manager in the options menu.";
        public const string BoardSelectScreenFeed = "Welcome to the board design select screen. Where you get to choose the desired design of flooring for the gamematch.";
        #endregion

        #region Menu Descriptions

        // Menu Descs
        public const string Menu0 = "Adjust stuff as you would adjust your nuts...\non a car... or your bolts.";
        public const string Menu2 = "Play with a maximum of 4 players on gamepads\nand 2 players on keyboard. Fight amongst \nyourselves or against computers in a wide variety \nof modes, rules, and styles of play.";
        public const string Menu3 = "Play the ancient game of survival. Where it's only\nyou against an infinite number of foes. They come\nin waves, each time getting bigger in size. Watch\nout not to get outnumbered or else you will die.";
        public const string Menu4 = "Host a game, join a game. Just play online against\nfriends or enemies. \n - 8 Max. Players. (2 players per connection)\n - No Server Browser or Host Rerouting yet";
        public const string Menu5 = "Edit your various settings, names, and favorite \ncharacters for profiles. You can also delete and \ncreate new profiles from here";
        public const string Menu6 = "View the credits of the people who helped make \nthis game what it is today.";
        public const string Menu7 = "View and compare the high scores set on Survival \nmode. If you're really in a competitive  mood you \ncan even compare your scores with all the billions \nof people on the net.";
        #endregion
    }
}
