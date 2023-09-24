namespace Tiptup300.Slaam;

// TODO
//
// So this class has a mix of static strings
// later on I need to have this loaded in resources, but this has to wait
// for release 2 when the resources engine is getting revamped
// 
public static class DialogStrings
{
   public static Dictionary<string, string> _ = new Dictionary<string, string>()
   {
      // common
      { "ContinueSelected", "> Continue <"},
      { "Continue", "Continue"},
      { "QuitSelected", "> Quit <"},
      { "Quit", "Quit"},
      { "SpeedUpName", "Speed UP!"},
      { "SpeedDoownName", "Speed Doown"},
      { "InversionName", "Inversion"},
      // storage
      { "StorageLocation", "Slaam! Saved Files"},
      { "ProfileFilename", "profiles.pro"},
      { "SurvivalScoresFilename", "survivalscores.g2g"},
      { "MatchSettingsFilename", "settings.cya"},
      // menu
      { "Start", "Start"},
      { "LocalGame", "Local Match"},
      { "OnlinePlay", "Online Play"},
      { "Survival", "Survival"},
      { "SearchForGames", "Search For Games"},
      { "JoinAGame", "Join a Game"},
      { "HostAGame", "Host a Game"},
      { "ManageProfiles", "Manage Profiles"},
      { "Options", "Options"},
      { "Credits", "Credits"},
      { "ViewHighScores", "High Score List"},
      // lobby
      { "CurrentPeers", "Current Peers: "},
      { "CurrentBoard", "Board: "},
      { "CreatedBy", "Created by "},
      { "Player", "Player "},
      // character select
      { "PressStartToJoin", "Press start to join."},
      { "SelectAProfile", "Select a profile."},
      { "PlayingAs", "Playing as "},
      // stats
      { "Played", "Played "},
      { "BestTime", "Best Time: "},
      { "Used", "Used "},
      { "Died", "Died "},
      { "Games", " Games"},
      { "Times", " Times"},
      { "Kills", " Kills"},
      { "Powerups", " Powerups"}
   };

}
