using Microsoft.Xna.Framework;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.States.MainMenu;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.PostGameStats.StatsBoards;

namespace Tiptup300.Slaam.States.PostGameStats;

public class StatsScreenPerformer : IPerformer<StatsScreenState>, IRenderer<StatsScreenState>
{
   public const int MAX_HIGHSCORES = 5;
   private readonly Color _statsColor = new Color(0, 0, 0, 125);

   private StatsScreenState _state = new StatsScreenState();

   private readonly ILogger _logger;
   private readonly IResources _resources;
   private readonly IRenderService _renderService;
   private readonly IInputService _inputService;
   private readonly Rectangle _statsRectangle;
   private readonly GameConfiguration _gameConfiguration;

   public StatsScreenPerformer(ILogger logger, IResources resources, IRenderService renderGraph, IInputService inputService, GameConfiguration gameConfiguration)
   {
      _logger = logger;
      _resources = resources;
      _renderService = renderGraph;
      _inputService = inputService;
      _gameConfiguration = gameConfiguration;

      _statsRectangle = new Rectangle(20, 110, _gameConfiguration.DRAWING_GAME_WIDTH - 40, _gameConfiguration.DRAWING_GAME_HEIGHT);
   }

   public void Initialize(StatsScreenRequestState request)
   {
      _state.ScoreCollection = request.ScoreCollection;
      _state.GameType = request.GameType;
   }

   public void InitializeState()
   {
      _state._statsButtons = setStatsButtons();
      if (_state.GameType == GameType.Classic)
      {
         _state.PlayerStats = new NormalStatsBoard(_state.ScoreCollection, _statsRectangle, _statsColor, _resources, _renderService, _state);
      }
      else if (_state.GameType == GameType.Spree || _state.GameType == GameType.TimedSpree)
      {
         _state.PlayerStats = new SpreeStatsBoard(_state.ScoreCollection, _statsRectangle, _statsColor, _resources, _renderService, _state);
      }
      else if (_state.GameType == GameType.Survival)
      {
         _state.PlayerStats = new SurvivalStatsBoard(_state.ScoreCollection, _statsRectangle, _statsColor, MAX_HIGHSCORES, _logger, _resources, _renderService, _state);
      }

      _state.PlayerStats.CalculateStats();
      _state.PlayerStats.ConstructGraph(0);

      if (_state.GameType != GameType.Survival)
      {

         _state.Kills = new KillsStatsBoard(_state.ScoreCollection, _statsRectangle, _statsColor, _resources, _renderService, _state);
         _state.Kills.CalculateStats();
         _state.Kills.ConstructGraph(0);

         _state.PvP = new PvPStatsBoard(_state.ScoreCollection, _statsRectangle, _statsColor, _resources, _renderService, _state);
         _state.PvP.CalculateStats();
         _state.PvP.ConstructGraph(0);

         string first = "First: ",
                second = "Second: ",
                third = "Third: ";

         for (int x = 0; x < _state.PlayerStats.MainBoard.Items.Count; x++)
         {
            if (_state.PlayerStats.MainBoard.Items[x].Details[1] == "First")
            {
               first += _state.PlayerStats.MainBoard.Items[x].Details[0] + ", ";
            }

            if (_state.PlayerStats.MainBoard.Items[x].Details[1] == "Second")
            {
               second += _state.PlayerStats.MainBoard.Items[x].Details[0] + ", ";
            }

            if (_state.PlayerStats.MainBoard.Items[x].Details[1] == "Third")
            {
               third += _state.PlayerStats.MainBoard.Items[x].Details[0] + ", ";
            }
         }

         if (second == "Second: ")
         {
            second = "";
         }
         else
         {
            second = second.Substring(0, second.Length - 2);
         }

         if (third == "Third: ")
         {
            third = "";
         }
         else
         {
            third = third.Substring(0, third.Length - 2);
         }


         _state.CurrentChar = new IntRange(0, 0, _state.PvP.MainBoard.Items.Count - 1);

      }
   }

   private CachedTexture[] setStatsButtons()
   {
      CachedTexture[] output;

      output = new CachedTexture[3];
      output[0] = _resources.GetTexture("StatsButton1");
      output[1] = _resources.GetTexture("StatsButton2");
      output[2] = _resources.GetTexture("StatsButton3");

      return output;
   }

   public IState Perform(StatsScreenState state)
   {
      if (state.GameType != GameType.Survival)
      {

         if (_inputService.GetPlayers()[0].PressedLeft)
         {
            state.CurrentPage.Sub(1);
         }

         if (_inputService.GetPlayers()[0].PressedRight)
         {
            state.CurrentPage.Add(1);
         }

         if (state.CurrentPage.Value == 2)
         {
            if (_inputService.GetPlayers()[0].PressedUp)
            {
               state.CurrentChar.Sub(1);
               state.PvP.ConstructGraph(state.CurrentChar.Value);
            }
            else if (_inputService.GetPlayers()[0].PressedDown)
            {
               state.CurrentChar.Add(1);
               state.PvP.ConstructGraph(state.CurrentChar.Value);
            }
         }

      }

      if (_inputService.GetPlayers()[0].PressedAction)
      {
         new MainMenuScreenState();
         //_screenDirector.ChangeTo<IMainMenuScreen>();
      }
      return state;
   }

   public void Render(StatsScreenState state)
   {
      _renderService.Render(batch =>
      {
         Vector2 Statsboard = new Vector2(_gameConfiguration.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("StatsBoard").Width / 2, _gameConfiguration.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("StatsBoard").Height / 2);

         for (int x = 0; x < 3; x++)
         {
            batch.Draw(state._statsButtons[x].Texture, Statsboard, x == state.CurrentPage.Value ? Color.LightSkyBlue : state.GameType == GameType.Survival ? Color.DarkGray : Color.White);
         }
         batch.Draw(_resources.GetTexture("StatsBoard").Texture, Statsboard, Color.White);

         if (state.CurrentPage.Value == 0)
         {
            state.PlayerStats.MainBoard.Draw(batch);
         }
         else if (state.CurrentPage.Value == 1)
         {
            state.Kills.MainBoard.Draw(batch);
         }
         else
         {
            state.PvP.MainBoard.Draw(batch);
         }
      });
   }
}
