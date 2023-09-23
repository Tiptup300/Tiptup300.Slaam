using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.PlayerProfiles;

/// <summary>
/// This is a game component that implements IUpdateable.
/// </summary>
public static class ProfileManager
{
   // TODO: This class is currently a singleton, it needs changed to a injected service.
   // TODO: This class needs cleaned up in a way that it's state is seperate. 
   // 
   public static bool Initialized { get; private set; }
   public static List<PlayerProfile> AllProfiles;
   public static RedirectionList<PlayerProfile> PlayableProfiles;
   public static RedirectionList<PlayerProfile> BotProfiles;
   private static Random rand = new Random();
   private static bool filefound = false;
   public static bool FirstTime = false;

   private static ILogger _logger => ServiceLocator.Instance.GetService<ILogger>();
   private static IResources _resources => ServiceLocator.Instance.GetService<IResources>();
   private static GameConfiguration _gameConfiguration => ServiceLocator.Instance.GetService<GameConfiguration>();

   public static void LoadProfiles()
   {
      XnaContentReader reader = new XnaContentReader(_logger, ServiceLocator.Instance.GetService<ProfileFileVersion>());
      reader.Initialize(DialogStrings.ProfileFilename);

      filefound = !reader.WasNotFound;

      AllProfiles = new List<PlayerProfile>();
      PlayableProfiles = new RedirectionList<PlayerProfile>(AllProfiles);
      BotProfiles = new RedirectionList<PlayerProfile>(AllProfiles);
      AllProfiles.Add(new PlayerProfile(0, 0, 0, "", _gameConfiguration.DEFAULT_PLAYER_NAME, false, 0, 0));

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
            {
               BotProfiles.Add(AllProfiles.Count - 1);
            }
            else
            {
               PlayableProfiles.Add(AllProfiles.Count - 1);
            }
         }
      }

      reader.Close();
      if (BotProfiles.Count != _resources.GetTextList("botnames").Count)
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
         for (int x = 0; x < _resources.GetTextList("botnames").Count; x++)
         {
            AllProfiles.Add(new PlayerProfile(_resources.GetTextList("botnames")[x].Replace("\r", ""), true));
            BotProfiles.Add(AllProfiles.Count - 1);
         }
      }

      SaveProfiles();

      FirstTime = !filefound;
   }

   public static void SaveProfiles()
   {
      XnaContentWriter writer = new XnaContentWriter(ServiceLocator.Instance.GetService<ProfileFileVersion>());
      writer.Initialize(DialogStrings.ProfileFilename);

      writer.Write(AllProfiles.Count - 1);

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
      int index = rand.Next(0, BotProfiles.Count);
      int ct = 0;
      do
      {
         index = rand.Next(0, BotProfiles.Count);
         ct++;

         if (ct > 100000)
            throw new Exception("Infinite Loop detected...");
      }
      while (BotProfiles[index].Used);

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
