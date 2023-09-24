using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.PlayerProfiles;

/// <summary>
/// This is a game component that implements IUpdateable.
/// </summary>
public class ProfileManager
{
   // TODO
   // Instance needs to be removed and it needs to be injected.
   // The issue is that many different classes hook into this. How can I do this piecemeal?
   // One file at a time.
   //
   public static ProfileManager Instance { get; private set; } = new ProfileManager
   (
      logger: ServiceLocator.Instance.GetService<ILogger>(),
      resources: ServiceLocator.Instance.GetService<IResources>(),
      gameConfiguration: ServiceLocator.Instance.GetService<GameConfiguration>()
   );

   public bool FirstTime = false;

   public List<PlayerProfile>? State_AllProfiles;
   public RedirectionList<PlayerProfile>? State_PlayableProfiles;
   public RedirectionList<PlayerProfile>? State_BotProfiles;

   private readonly ILogger _logger;
   private readonly IResources _resources;
   private readonly GameConfiguration _gameConfiguration;
   private readonly Random _random = new Random();

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
      reader.Initialize(DialogStrings._["ProfileFilename"]);

      var filefound = !reader.WasNotFound;

      State_AllProfiles = new List<PlayerProfile>();
      State_PlayableProfiles = new RedirectionList<PlayerProfile>(State_AllProfiles);
      State_BotProfiles = new RedirectionList<PlayerProfile>(State_AllProfiles);
      State_AllProfiles.Add(new PlayerProfile(0, 0, 0, "", _gameConfiguration.DEFAULT_PLAYER_NAME, false, 0, 0));

      State_PlayableProfiles.Add(0);

      if (reader.IsWrongVersion())
      {
         // Wrong Version, do nothing...
      }
      else
      {
         int ProfileAmt = reader.ReadInt32();
         for (int x = 0; x < ProfileAmt; x++)
         {
            State_AllProfiles.Add(new PlayerProfile(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadString(), reader.ReadString(), reader.ReadBool(), reader.ReadInt32(), reader.ReadInt32()));
            if (State_AllProfiles[State_AllProfiles.Count - 1].IsBot)
            {
               State_BotProfiles.Add(State_AllProfiles.Count - 1);
            }
            else
            {
               State_PlayableProfiles.Add(State_AllProfiles.Count - 1);
            }
         }
      }

      reader.Close();
      if (State_BotProfiles.Count != _resources.GetTextList("botnames").Count)
      {
         for (int x = 0; x < State_AllProfiles.Count; x++)
         {
            if (State_AllProfiles[x].IsBot)
            {
               State_AllProfiles.RemoveAt(x);
               x--;
            }
         }
         State_BotProfiles = new RedirectionList<PlayerProfile>(State_AllProfiles);
         for (int x = 0; x < _resources.GetTextList("botnames").Count; x++)
         {
            State_AllProfiles.Add(new PlayerProfile(_resources.GetTextList("botnames")[x].Replace("\r", ""), true));
            State_BotProfiles.Add(State_AllProfiles.Count - 1);
         }
      }

      SaveProfiles();

      FirstTime = !filefound;
   }

   public void SaveProfiles()
   {
      XnaContentWriter writer = new XnaContentWriter(ServiceLocator.Instance.GetService<ProfileFileVersion>());
      writer.Initialize(DialogStrings._["ProfileFilename"]);

      writer.Write(State_AllProfiles.Count - 1);

      for (int x = 1; x < State_AllProfiles.Count; x++)
      {
         writer.Write(State_AllProfiles[x].TotalKills);
         writer.Write(State_AllProfiles[x].TotalGames);
         writer.Write(State_AllProfiles[x].TotalDeaths);
         writer.Write(State_AllProfiles[x].Skin);
         writer.Write(State_AllProfiles[x].Name);
         writer.Write(State_AllProfiles[x].IsBot);
         writer.Write(State_AllProfiles[x].TotalPowerups);
         writer.Write((int)State_AllProfiles[x].BestGame.TotalMilliseconds);
      }

      writer.Close();
   }

   public void AddNewProfile(PlayerProfile prof)
   {
      State_AllProfiles.Add(prof);
      if (prof.IsBot)
         State_BotProfiles.Add(State_AllProfiles.Count - 1);
      else
         State_PlayableProfiles.Add(State_AllProfiles.Count - 1);
   }

   public int GetBotProfile()
   {
      int index = _random.Next(0, State_BotProfiles.Count);
      int ct = 0;
      do
      {
         index = _random.Next(0, State_BotProfiles.Count);
         ct++;

         if (ct > 100000)
            throw new Exception("Infinite Loop detected...");
      }
      while (State_BotProfiles[index].Used);

      State_BotProfiles[index].Used = true;

      return State_BotProfiles.GetRealIndex(index);
   }

   public void ResetAllBots()
   {
      for (int x = 0; x < State_BotProfiles.Count; x++)
         State_BotProfiles[x].Used = false;
   }

   public void ResetBot(int index)
   {
      State_AllProfiles[index].Used = false;
   }

   public void RemovePlayer(int index)
   {
      State_AllProfiles.RemoveAt(index);
      SaveProfiles();
      LoadProfiles();
   }
}
