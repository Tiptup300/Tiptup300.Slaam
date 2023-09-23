using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.PlayerProfiles;

/// <summary>
/// This is a game component that implements IUpdateable.
/// </summary>
public class ProfileManager
{
   public static ProfileManager Instance { get; private set; } = new ProfileManager
   (
      logger: ServiceLocator.Instance.GetService<ILogger>(),
      resources: ServiceLocator.Instance.GetService<IResources>(),
      gameConfiguration: ServiceLocator.Instance.GetService<GameConfiguration>()
   );

   // TODO: This class is currently a singleton, it needs changed to a injected service.
   // TODO: This class needs cleaned up in a way that it's state is seperate. 
   // 
   public bool FirstTime = false;

   public List<PlayerProfile>? state_AllProfiles;
   public RedirectionList<PlayerProfile>? state_PlayableProfiles;
   public RedirectionList<PlayerProfile>? state_BotProfiles;


   private readonly Random _random = new Random();
   private readonly ILogger _logger;
   private readonly IResources _resources;
   private readonly GameConfiguration _gameConfiguration;

   public ProfileManager(
      ILogger logger, IResources resources, GameConfiguration gameConfiguration)
   {
      _logger = logger;
      _resources = resources;
      _gameConfiguration = gameConfiguration;
   }

   public void LoadProfiles()
   {
      XnaContentReader reader = new XnaContentReader(_logger, ServiceLocator.Instance.GetService<ProfileFileVersion>());
      reader.Initialize(DialogStrings.ProfileFilename);

      var filefound = !reader.WasNotFound;

      state_AllProfiles = new List<PlayerProfile>();
      state_PlayableProfiles = new RedirectionList<PlayerProfile>(state_AllProfiles);
      state_BotProfiles = new RedirectionList<PlayerProfile>(state_AllProfiles);
      state_AllProfiles.Add(new PlayerProfile(0, 0, 0, "", _gameConfiguration.DEFAULT_PLAYER_NAME, false, 0, 0));

      state_PlayableProfiles.Add(0);

      if (reader.IsWrongVersion())
      {
         // Wrong Version, do nothing...
      }
      else
      {
         int ProfileAmt = reader.ReadInt32();
         for (int x = 0; x < ProfileAmt; x++)
         {
            state_AllProfiles.Add(new PlayerProfile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadString(), reader.ReadString(), reader.ReadBool(), reader.ReadInt32(), reader.ReadInt32()));
            if (state_AllProfiles[state_AllProfiles.Count - 1].IsBot)
            {
               state_BotProfiles.Add(state_AllProfiles.Count - 1);
            }
            else
            {
               state_PlayableProfiles.Add(state_AllProfiles.Count - 1);
            }
         }
      }

      reader.Close();
      if (state_BotProfiles.Count != _resources.GetTextList("botnames").Count)
      {
         for (int x = 0; x < state_AllProfiles.Count; x++)
         {
            if (state_AllProfiles[x].IsBot)
            {
               state_AllProfiles.RemoveAt(x);
               x--;
            }
         }
         state_BotProfiles = new RedirectionList<PlayerProfile>(state_AllProfiles);
         for (int x = 0; x < _resources.GetTextList("botnames").Count; x++)
         {
            state_AllProfiles.Add(new PlayerProfile(_resources.GetTextList("botnames")[x].Replace("\r", ""), true));
            state_BotProfiles.Add(state_AllProfiles.Count - 1);
         }
      }

      SaveProfiles();

      FirstTime = !filefound;
   }

   public void SaveProfiles()
   {
      XnaContentWriter writer = new XnaContentWriter(ServiceLocator.Instance.GetService<ProfileFileVersion>());
      writer.Initialize(DialogStrings.ProfileFilename);

      writer.Write(state_AllProfiles.Count - 1);

      for (int x = 1; x < state_AllProfiles.Count; x++)
      {
         writer.Write(state_AllProfiles[x].TotalKills);
         writer.Write(state_AllProfiles[x].TotalGames);
         writer.Write(state_AllProfiles[x].TotalDeaths);
         writer.Write(state_AllProfiles[x].Skin);
         writer.Write(state_AllProfiles[x].Name);
         writer.Write(state_AllProfiles[x].IsBot);
         writer.Write(state_AllProfiles[x].TotalPowerups);
         writer.Write((int)state_AllProfiles[x].BestGame.TotalMilliseconds);
      }

      writer.Close();
   }

   public void AddNewProfile(PlayerProfile prof)
   {
      state_AllProfiles.Add(prof);
      if (prof.IsBot)
         state_BotProfiles.Add(state_AllProfiles.Count - 1);
      else
         state_PlayableProfiles.Add(state_AllProfiles.Count - 1);
   }

   public int GetBotProfile()
   {
      int index = _random.Next(0, state_BotProfiles.Count);
      int ct = 0;
      do
      {
         index = _random.Next(0, state_BotProfiles.Count);
         ct++;

         if (ct > 100000)
            throw new Exception("Infinite Loop detected...");
      }
      while (state_BotProfiles[index].Used);

      state_BotProfiles[index].Used = true;

      return state_BotProfiles.GetRealIndex(index);
   }

   public void ResetAllBots()
   {
      for (int x = 0; x < state_BotProfiles.Count; x++)
         state_BotProfiles[x].Used = false;
   }

   public void ResetBot(int index)
   {
      state_AllProfiles[index].Used = false;
   }

   public void RemovePlayer(int index)
   {
      state_AllProfiles.RemoveAt(index);
      SaveProfiles();
      LoadProfiles();
   }
}
