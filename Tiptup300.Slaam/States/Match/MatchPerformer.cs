using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.CharacterSelect;
using Tiptup300.Slaam.States.Lobby;
using Tiptup300.Slaam.States.Match.Actors;
using Tiptup300.Slaam.States.Match.Boards;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.Match.Powerups;
using Tiptup300.Slaam.States.Match.Scoreboard;
using Tiptup300.Slaam.States.PostGameStats;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.States.Match;

public class MatchPerformer : IPerformer<MatchState>
{

   private readonly IResources _resources;
   private readonly IGraphicsStateService _graphics;
   private readonly IResolver<MatchScoreboardRequest, MatchScoreboard> _gameScreenScoreBoardResolver;
   private readonly ILogger _logger;
   private readonly IInputService _inputService;
   private readonly IFrameTimeService _frameTimeService;
   private readonly IRenderService _renderService;

   public MatchPerformer(
       IResources resources,
       IGraphicsStateService graphicsState,
       IResolver<MatchScoreboardRequest, MatchScoreboard> gameScreenScoreBoardResolver,
       ILogger logger,
       IInputService inputService,
       IFrameTimeService frameTimeService,
       IRenderService renderService)
   {
      _resources = resources;
      _graphics = graphicsState;
      _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
      _logger = logger;
      _inputService = inputService;
      _frameTimeService = frameTimeService;
      _renderService = renderService;
   }

   public void InitializeState() { }

   private Vector2 calcFinalBoardPosition()
   {
      int width = (int)(_graphics.Get().PreferredBackBufferWidth / 2f);
      int height = (int)(_graphics.Get().PreferredBackBufferHeight / 2f);
      int boardWidth = GameGlobals.BOARD_WIDTH * GameGlobals.TILE_SIZE;
      int boardHeight = GameGlobals.BOARD_HEIGHT * GameGlobals.TILE_SIZE;

      return new Vector2(width - boardWidth / 2f, height - boardHeight / 2f);
   }

   private void setupPauseMenu(MatchState state)
   {
   }

   private void SetupTheBoard(string BoardLoc, MatchState state)
   {
      if (state.GameType == GameType.Survival)
      {
         survival_SetupTheBoard(BoardLoc, state);
      }
      else
      {
         state.Tileset = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + BoardLoc);

         for (int x = 0; x < state.SetupCharacters.Count; x++)
         {
            if (state.SetupCharacters[x].PlayerType == PlayerType.Player)
            {
               state.Characters.Add(
                   new CharacterActor(
                       SlaamGame.Content.Load<Texture2D>("content\\skins\\" + state.SetupCharacters[x].SkinLocation),
                       state.SetupCharacters[x].CharacterProfileIndex,
                       new Vector2(-100, -100),
                       _inputService.GetPlayers()[(int)state.SetupCharacters[x].PlayerIndex],
                       state.SetupCharacters[x].PlayerColor,
                       x,
                       ServiceLocator.Instance.GetService<IResources>(),
                       _frameTimeService,
                       state.CurrentMatchSettings));
            }
            else
            {
               ProfileManager.Instance.AllProfiles[state.SetupCharacters[x].CharacterProfileIndex].Skin = state.SetupCharacters[x].SkinLocation;
               state.Characters.Add(
                   new BotActor(
                       SlaamGame.Content.Load<Texture2D>("content\\skins\\" + state.SetupCharacters[x].SkinLocation),
                       state.SetupCharacters[x].CharacterProfileIndex,
                       new Vector2(-100, -100),
                       this,
                       state.SetupCharacters[x].PlayerColor,
                       state.Characters.Count,
                       ServiceLocator.Instance.GetService<IResources>(),
                       _frameTimeService,
                       state.CurrentMatchSettings));
            }

            state.Scoreboards.Add(
                _gameScreenScoreBoardResolver.Resolve(
                    new MatchScoreboardRequest(
                        Vector2.Zero,
                        state.Characters[state.Characters.Count - 1],
                        state.GameType)));
         }
      }
   }
   private void survival_SetupTheBoard(string BoardLoc, MatchState state)
   {
      state.GameType = GameType.Survival;
      state.CurrentMatchSettings.GameType = GameType.Survival;
      state.CurrentMatchSettings.SpeedMultiplyer = 1f;
      state.CurrentMatchSettings.RespawnTime = new TimeSpan(0, 0, 8);
      state.CurrentMatchSettings.LivesAmt = 1;
      state.Tileset = LobbyScreenFunctions.LoadQuickBoard();

      state.Characters.Add(new CharacterActor(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + state.SetupCharacters[0].SkinLocation), state.SetupCharacters[0].CharacterProfileIndex, new Vector2(-100, -100), _inputService.GetPlayers()[0], Color.White, 0, ServiceLocator.Instance.GetService<IResources>(), _frameTimeService, state.CurrentMatchSettings));
      state.Scoreboards.Add(
          _gameScreenScoreBoardResolver.Resolve(
              new MatchScoreboardRequest(
                  new Vector2(-250, 10),
                  state.Characters[0],
                  state.GameType)));
   }

   public IState Perform(MatchState state)
   {
      if (state.EndGameSelected)
      {
         return endGame(state);
      }
      if (state.GameType == GameType.Survival)
      {
         survival_Perform(state);
      }
      if (state.IsPaused)
      {

         return state;
      }

      state.Timer.Update(state);
      updateScoreBoards(state);

      if (state.CurrentGameStatus == GameStatus.MovingBoard)
      {
         updateMovingBoardState(state);
      }
      else if (state.CurrentGameStatus == GameStatus.Respawning)
      {
         updateRespawningGameState(state);
      }
      else if (state.CurrentGameStatus == GameStatus.Waiting)
      {
         updateWaitingGameState(state);
      }
      else if (state.CurrentGameStatus == GameStatus.Playing)
      {
         updatePlayingGameState(state);
      }
      else if (state.CurrentGameStatus == GameStatus.Over)
      {
         updateOverGameState(state);
      }
      return state;
   }
   public void survival_Perform(MatchState _state)
   {
      if (_state.CurrentGameStatus == GameStatus.Playing)
      {
         _state.SurvivalState.TimeToAddBot.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         if (_state.SurvivalState.TimeToAddBot.Active)
         {
            for (int x = 0; x < _state.SurvivalState.BotsToAdd + 1; x++)
            {
               survival_AddNewBot(_state);
               _state.SurvivalState.BotsAdded++;

               if (_state.Rand.Next(0, _state.SurvivalState.BotsAdded - 1) == _state.SurvivalState.BotsAdded)
               {
                  _state.SurvivalState.BotsToAdd++;
               }
            }
         }

         for (int x = 0; x < _state.Characters.Count; x++)
         {
            if (_state.Characters[x] != null && _state.Characters[x].Lives == 0)
            {
               _state.Characters[x] = null;
               _state.NullChars++;
            }
         }
      }

      bool temp = _state.CurrentGameStatus == GameStatus.Waiting;
      if (_state.CurrentGameStatus == GameStatus.Playing && temp)
      {
         survival_AddNewBot(_state);
      }
   }
   private void survival_AddNewBot(MatchState _state)
   {
      _state.Characters.Add(
          new BotActor(
              SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SkinLoadingFunctions.ReturnRandSkin(_logger)),
              ProfileManager.Instance.GetBotProfile(),
              new Vector2(-200, -200),
              this,
              Color.Black,
              _state.Characters.Count,
              ServiceLocator.Instance.GetService<IResources>(),
              _frameTimeService,
              _state.CurrentMatchSettings));

      ProfileManager.Instance.ResetAllBots();
      MatchFunctions.RespawnCharacter(_state, _state.Characters.Count - 1);
   }

   private void updateOverGameState(MatchState state)
   {
      state.IsTiming = false;
      state.ReadySetGoThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (state.ReadySetGoThrottle.Active)
      {
         endGame(state);
      }
   }
   private void updatePlayingGameState(MatchState state)
   {
      for (int x = 0; x < state.Characters.Count; x++)
      {
         if (state.Characters[x] != null)
         {
            int X1 = (int)((state.Characters[x].Position.X - state.Boardpos.X) % GameGlobals.TILE_SIZE);
            int Y1 = (int)((state.Characters[x].Position.Y - state.Boardpos.Y) % GameGlobals.TILE_SIZE);
            int X = (int)((state.Characters[x].Position.X - state.Boardpos.X - X1) / GameGlobals.TILE_SIZE);
            int Y = (int)((state.Characters[x].Position.Y - state.Boardpos.Y - Y1) / GameGlobals.TILE_SIZE);
            state.Characters[x].Update(new Vector2(X, Y), new Vector2(X1, Y1), state);
            if (state.Characters[x].CurrentState == CharacterActor.CharacterState.Respawning)
            {
               MatchFunctions.RespawnCharacter(state, x);
            }
         }
      }
      for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
      {
         for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
         {
            state.Tiles[x, y].Update(state);
         }
      }
      state.PowerupTime.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (state.PowerupTime.Active)
      {
         bool found = true;
         int newx = state.Rand.Next(0, GameGlobals.BOARD_WIDTH);
         int newy = state.Rand.Next(0, GameGlobals.BOARD_HEIGHT);
         int ct = 0;

         while (state.Tiles[newx, newy].CurrentTileCondition != TileCondition.Normal)
         {
            newx = state.Rand.Next(0, GameGlobals.BOARD_WIDTH);
            newy = state.Rand.Next(0, GameGlobals.BOARD_HEIGHT);
            ct++;
            if (ct > 100)
            {
               found = false;
               break;
            }
         }
         if (found)
         {
            state.Tiles[newx, newy].MarkWithPowerup(PowerupManager.Instance.GetRandomPowerup());
         }
      }
   }
   private void updateWaitingGameState(MatchState state)
   {
      state.ReadySetGoThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (state.ReadySetGoThrottle.Active)
      {
         state.ReadySetGoPart++;
         if (state.ReadySetGoPart > 2)
         {
            state.CurrentGameStatus = GameStatus.Playing;
            state.ReadySetGoPart = 2;
            state.ReadySetGoThrottle.Reset();
            state.IsTiming = true;
         }
      }
   }
   private void updateRespawningGameState(MatchState state)
   {
      state.ReadySetGoThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (state.ReadySetGoThrottle.Active)
      {
         state.Scoreboards[state.ReadySetGoPart].Moving = true;
         MatchFunctions.RespawnCharacter(state, state.ReadySetGoPart++);
         if (state.ReadySetGoPart == state.Characters.Count)
         {
            state.CurrentGameStatus = GameStatus.Waiting;
            state.ReadySetGoThrottle.Threshold = new TimeSpan(0, 0, 0, 1, 300);
            state.ReadySetGoThrottle.Reset();
            state.ReadySetGoThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            state.ReadySetGoPart = 0;
            state.Timer.Moving = true;
         }
      }
   }
   private void updateMovingBoardState(MatchState state)
   {
      state.Boardpos = new Vector2(state.Boardpos.X, state.Boardpos.Y + _frameTimeService.GetLatestFrame().MovementFactor * (10f / 100f));

      if (state.Boardpos.Y >= calcFinalBoardPosition().Y)
      {
         state.Boardpos = calcFinalBoardPosition();
         state.CurrentGameStatus = GameStatus.Respawning;
      }
      for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
      {
         for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
         {
            state.Tiles[x, y].ResetTileLocation(state.Boardpos, new Vector2(x, y));
         }
      }
   }
   private void updateScoreBoards(MatchState state)
   {
      for (int x = 0; x < state.Scoreboards.Count; x++)
      {
         state.Scoreboards[x].Update();
      }
   }

   public void Render(MatchState state)
   {
      _renderService.Render(batch =>
      {
         if (state.IsPaused)
         {
            return;
         }
         for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
         {
            for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
            {
               state.Tiles[x, y].Draw(batch);
            }
         }

         float PlayersDrawn = 0, CurrY = 0;

         int CurrPlayer = -1;

         while (PlayersDrawn < state.Characters.Count - state.NullChars)
         {
            CurrY = 1280;
            CurrPlayer = -1;
            for (int x = 0; x < state.Characters.Count; x++)
            {
               if (state.Characters[x] != null && !state.Characters[x].Drawn && state.Characters[x].Position.Y <= CurrY)
               {
                  CurrY = state.Characters[x].Position.Y;
                  CurrPlayer = x;
               }
            }
            state.Characters[CurrPlayer].Drawn = true;
            state.Characters[CurrPlayer].Draw(batch);
            PlayersDrawn++;
         }

         resetCharactersDrawnStatus(state);

         for (int x = 0; x < state.Characters.Count; x++)
         {
            if (state.Characters[x] != null)
            {
               state.Characters[x].Drawn = false;
            }
         }
         if (state.CurrentGameStatus == GameStatus.Waiting || state.CurrentGameStatus == GameStatus.Over)
         {
            batch.Draw(_resources.GetTexture("ReadySetGo").Texture, new Vector2((float)state.Rand.NextDouble() * (1 + state.ReadySetGoPart) + GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ReadySetGo").Width / 2, (float)state.Rand.NextDouble() * (1 + state.ReadySetGoPart) + GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ReadySetGo").Width / 8), new Rectangle(0, state.ReadySetGoPart * (_resources.GetTexture("ReadySetGo").Height / 4), _resources.GetTexture("ReadySetGo").Width, _resources.GetTexture("ReadySetGo").Height / 4), Color.White);
         }
      });
   }
   private void resetCharactersDrawnStatus(MatchState state)
   {
      state.Characters
          .Where(character => character != null)
          .ToList()
          .ForEach(character => character.Drawn = false);
   }

   private void unloadContent()
   {
      _resources.GetTexture("ReadySetGo").Unload();
      _resources.GetTexture("BattleBG").Unload();
   }





   private IState endGame(MatchState _state)
   {
      if (_state.GameType == GameType.Survival)
      {
         return survival_EndGame(_state);
      }
      else
      {
         _state.ScoreKeeper.CalcTotals(_state);
         for (int x = 0; x < _state.Characters.Count; x++)
         {
            _state.Characters[x].SaveProfileData();
         }
         ProfileManager.Instance.SaveProfiles();
         return new StatsScreenRequestState(_state.ScoreKeeper, _state.GameType);
      }
   }
   private IState survival_EndGame(MatchState _state)
   {
      if (ProfileManager.Instance.AllProfiles[_state.Characters[0].ProfileIndex].BestGame < _state.Timer.CurrentGameTime)
      {
         ProfileManager.Instance.AllProfiles[_state.Characters[0].ProfileIndex].BestGame = _state.Timer.CurrentGameTime;
      }
      ProfileManager.Instance.SaveProfiles();
      return new StatsScreenRequestState(_state.ScoreKeeper, _state.GameType);
   }
}
