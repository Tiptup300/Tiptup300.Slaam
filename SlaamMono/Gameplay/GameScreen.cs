using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.PlayerProfiles;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;
using System.Collections.Generic;
using System.Linq;
using ZBlade;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay
{
    public class GameScreen : ILogic
    {
        #region BOARD_STUFF
        // board
        public Tile[,] Tiles;
        protected Texture2D Tileset;
        private Vector2 _boardpos;

        private Vector2 calcFinalBoardPosition()
        {
            int width = (int)(_graphics.Get().PreferredBackBufferWidth / 2f);
            int height = (int)(_graphics.Get().PreferredBackBufferHeight / 2f);
            int boardWidth = GameGlobals.BOARD_WIDTH * GameGlobals.TILE_SIZE;
            int boardHeight = GameGlobals.BOARD_HEIGHT * GameGlobals.TILE_SIZE;

            return new Vector2(width - boardWidth / 2f, height - boardHeight / 2f);
        }
        public void ShortenBoard()
        {
            TimeSpan ShortenTime = new TimeSpan(0, 0, 0, 2);
            if (_boardsize < 6)
            {
                markBoardOutline();
                _boardsize++;
            }
            _stepsRemaining--;
            if (_stepsRemaining == 0)
            {
                CurrentGameStatus = GameStatus.Over;
                ReadySetGoPart = 3;
                ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            }
        }
        public Vector2 InterpretCoordinates(Vector2 pos, bool flip)
        {
            if (!flip)
                return new Vector2(_boardpos.X + pos.X * GameGlobals.TILE_SIZE, _boardpos.Y + pos.Y * GameGlobals.TILE_SIZE);
            else
            {

                int X1 = (int)((pos.X - _boardpos.X) % GameGlobals.TILE_SIZE);
                int Y1 = (int)((pos.Y - _boardpos.Y) % GameGlobals.TILE_SIZE);
                int X = (int)((pos.X - _boardpos.X - X1) / GameGlobals.TILE_SIZE);
                int Y = (int)((pos.Y - _boardpos.Y - Y1) / GameGlobals.TILE_SIZE);

                if (pos.X < _boardpos.X)
                    X = -1;
                if (pos.Y < _boardpos.Y)
                    Y = -1;

                return new Vector2(X, Y);
            }
        }
        private int _boardsize = 0;

        #endregion

        public static GameScreen Instance;


        public MenuItemTree main = new MenuItemTree();


        public GameType ThisGameType;
        public List<CharacterActor> Characters;

        protected GameScreenTimer Timer;
        protected List<CharacterShell> SetupCharacters;
        protected int NullChars = 0;
        protected List<Scoreboard> Scoreboards;
        protected Random Rand = new Random();
        protected GameStatus CurrentGameStatus;
        protected int ReadySetGoPart = 0;
        protected Timer ReadySetGoThrottle;
        protected MatchScoreCollection ScoreKeeper;

        private bool _timing = false;
        private bool _paused = false;
        private int _killsToWin = 0;
        private Timer _powerupTime;
        private float _spreeStepSize;
        private float _spreeCurrentStep;
        private int _spreeHighestKillCount;
        private int _stepsRemaining;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;
        private readonly IResolver<StatsScreenRequest, StatsScreen> _statsScreenRequest;

        public GameScreen(
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphicsState,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver,
            IResolver<StatsScreenRequest, StatsScreen> statsScreenRequest)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphicsState;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
            _statsScreenRequest = statsScreenRequest;
        }

        public void Initialize(GameScreenRequest gameScreenRequest)
        {
            SetupCharacters = gameScreenRequest.SetupCharacters;

            _powerupTime = new Timer(new TimeSpan(0, 0, 0, 15));
            ReadySetGoThrottle = new Timer(new TimeSpan(0, 0, 0, 0, 325));
            Scoreboards = new List<Scoreboard>();
            SetupCharacters = new List<CharacterShell>();
            Characters = new List<CharacterActor>();
            Tiles = new Tile[GameGlobals.BOARD_WIDTH, GameGlobals.BOARD_HEIGHT];
            CurrentGameStatus = GameStatus.Waiting;
            ThisGameType = CurrentMatchSettings.GameType;
            SetupTheBoard(CurrentMatchSettings.BoardLocation);
            CurrentGameStatus = GameStatus.MovingBoard;
            _resources.GetTexture("ReadySetGo").Load();
            _resources.GetTexture("BattleBG").Load();
        }

        public void InitializeState()
        {
            _boardpos = calcFinalBoardPosition();
            _boardpos.Y = -Tileset.Height;

            Timer = new GameScreenTimer(
                new Vector2(1024, 0),
                this,
                x_Di.Get<IResources>());

            FeedManager.FeedsActive = false;
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y] = new Tile(
                        _boardpos,
                        new Vector2(x, y),
                        Tileset,
                        x_Di.Get<IResources>(),
                        x_Di.Get<IRenderGraph>());
                }
            }
            ScoreKeeper = new MatchScoreCollection(this);
            ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            BackgroundManager.ChangeBG(BackgroundType.BattleScreen);
            if (ThisGameType == GameType.Classic)
            {
                _stepsRemaining = SetupCharacters.Count - 1;
            }
            else if (ThisGameType == GameType.TimedSpree)
            {
                _stepsRemaining = 7;
            }
            else if (ThisGameType == GameType.Spree)
            {
                //CurrentMatchSettings.KillsToWin = 7;
                _stepsRemaining = 100;
                _killsToWin = CurrentMatchSettings.KillsToWin;
                _spreeStepSize = 10;
                _spreeCurrentStep = 0;
            }

            setupPauseMenu();
        }

        private void setupPauseMenu()
        {
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;
            MenuTextItem resume = new MenuTextItem("Resume Game");
            resume.Activated += delegate
            {
                _paused = false;
                BackgroundManager.ChangeBG(BackgroundType.BattleScreen);
                SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            };
            MenuTextItem quit = new MenuTextItem("Quit Game");

            quit.Activated += delegate
            {
                SlaamGame.mainBlade.Status = BladeStatus.Hidden;
                EndGame();
            };

            main.Nodes.Add(resume);
            main.Nodes.Add(quit);

            SlaamGame.mainBlade.TopMenu = main;
        }

        public virtual void SetupTheBoard(string BoardLoc)
        {
            Tileset = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + BoardLoc);

            for (int x = 0; x < SetupCharacters.Count; x++)
            {
                if (SetupCharacters[x].Type == PlayerType.Player)
                {
                    Characters.Add(
                        new CharacterActor(
                            SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupCharacters[x].SkinLocation),
                            SetupCharacters[x].CharProfile,
                            new Vector2(-100, -100),
                            InputComponent.Players[(int)SetupCharacters[x].PlayerIDX],
                            SetupCharacters[x].PlayerColor,
                            x,
                            x_Di.Get<IResources>()));
                }
                else
                {
                    ProfileManager.AllProfiles[SetupCharacters[x].CharProfile].Skin = SetupCharacters[x].SkinLocation;
                    Characters.Add(
                        new BotActor(
                            SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupCharacters[x].SkinLocation),
                            SetupCharacters[x].CharProfile,
                            new Vector2(-100, -100),
                            this,
                            SetupCharacters[x].PlayerColor,
                            Characters.Count,
                            x_Di.Get<IResources>()));
                }

                Scoreboards.Add(
                    _gameScreenScoreBoardResolver.Resolve(
                        new ScoreboardRequest(
                            Vector2.Zero,
                            Characters[Characters.Count - 1],
                            ThisGameType)));

            }
        }

        public virtual void UpdateState()
        {
            if (_paused)
            {
                return;
            }

            Timer.Update(_timing);
            updateScoreBoards();

            if (CurrentGameStatus == GameStatus.MovingBoard)
            {
                updateMovingBoardState();
            }
            else if (CurrentGameStatus == GameStatus.Respawning)
            {
                updateRespawningGameState();
            }
            else if (CurrentGameStatus == GameStatus.Waiting)
            {
                updateWaitingGameState();
            }
            else if (CurrentGameStatus == GameStatus.Playing)
            {
                updatePlayingGameState();
            }
            else if (CurrentGameStatus == GameStatus.Over)
            {
                updateOverGameState();
            }

        }
        private void updateOverGameState()
        {
            _timing = false;
            ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            if (ReadySetGoThrottle.Active)
            {
                EndGame();
            }
        }
        private void updatePlayingGameState()
        {
            for (int x = 0; x < Characters.Count; x++)
            {
                if (Characters[x] != null)
                {
                    int X1 = (int)((Characters[x].Position.X - _boardpos.X) % GameGlobals.TILE_SIZE);
                    int Y1 = (int)((Characters[x].Position.Y - _boardpos.Y) % GameGlobals.TILE_SIZE);
                    int X = (int)((Characters[x].Position.X - _boardpos.X - X1) / GameGlobals.TILE_SIZE);
                    int Y = (int)((Characters[x].Position.Y - _boardpos.Y - Y1) / GameGlobals.TILE_SIZE);
                    Characters[x].Update(Tiles, new Vector2(X, Y), new Vector2(X1, Y1));
                    if (Characters[x].CurrentState == CharacterActor.CharacterState.Respawning)
                    {
                        RespawnChar(x);
                    }
                }
            }
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y].Update();
                }
            }
            _powerupTime.Update(FrameRateDirector.MovementFactorTimeSpan);
            if (_powerupTime.Active)
            {
                bool found = true;
                int newx = Rand.Next(0, GameGlobals.BOARD_WIDTH);
                int newy = Rand.Next(0, GameGlobals.BOARD_HEIGHT);
                int ct = 0;

                while (Tiles[newx, newy].CurrentTileCondition != TileCondition.Normal)
                {
                    newx = Rand.Next(0, GameGlobals.BOARD_WIDTH);
                    newy = Rand.Next(0, GameGlobals.BOARD_HEIGHT);
                    ct++;
                    if (ct > 100)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    Tiles[newx, newy].MarkWithPowerup(PowerupManager.Instance.GetRandomPowerup());
                }
            }
        }
        private void updateWaitingGameState()
        {
            ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            if (ReadySetGoThrottle.Active)
            {
                ReadySetGoPart++;
                if (ReadySetGoPart > 2)
                {
                    CurrentGameStatus = GameStatus.Playing;
                    ReadySetGoPart = 2;
                    ReadySetGoThrottle.Reset();
                    _timing = true;
                }
            }
        }
        private void updateRespawningGameState()
        {
            ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            if (ReadySetGoThrottle.Active)
            {
                Scoreboards[ReadySetGoPart].Moving = true;
                RespawnChar(ReadySetGoPart++);
                if (ReadySetGoPart == Characters.Count)
                {
                    CurrentGameStatus = GameStatus.Waiting;
                    ReadySetGoThrottle.Threshold = new TimeSpan(0, 0, 0, 1, 300);
                    ReadySetGoThrottle.Reset();
                    ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
                    ReadySetGoPart = 0;
                    Timer.Moving = true;
                }
            }
        }
        private void updateMovingBoardState()
        {
            _boardpos.Y += FrameRateDirector.MovementFactor * (10f / 100f);

            if (_boardpos.Y >= calcFinalBoardPosition().Y)
            {
                _boardpos = calcFinalBoardPosition();
                CurrentGameStatus = GameStatus.Respawning;
            }
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y].ResetTileLoc(_boardpos, new Vector2(x, y));
                }
            }
        }
        private void updateScoreBoards()
        {
            for (int x = 0; x < Scoreboards.Count; x++)
            {
                Scoreboards[x].Update();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (_paused)
            {
                return;
            }
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    Tiles[x, y].Draw(batch);
                }
            }

            float PlayersDrawn = 0, CurrY = 0;

            int CurrPlayer = -1;

            while (PlayersDrawn < Characters.Count - NullChars)
            {
                CurrY = 1280;
                CurrPlayer = -1;
                for (int x = 0; x < Characters.Count; x++)
                {
                    if (Characters[x] != null && !Characters[x].Drawn && Characters[x].Position.Y <= CurrY)
                    {
                        CurrY = Characters[x].Position.Y;
                        CurrPlayer = x;
                    }
                }
                Characters[CurrPlayer].Drawn = true;
                Characters[CurrPlayer].Draw(batch);
                PlayersDrawn++;
            }

            resetCharactersDrawnStatus();

            for (int x = 0; x < Characters.Count; x++)
            {
                if (Characters[x] != null)
                {
                    Characters[x].Drawn = false;
                }
            }
            if (CurrentGameStatus == GameStatus.Waiting || CurrentGameStatus == GameStatus.Over)
            {
                batch.Draw(_resources.GetTexture("ReadySetGo").Texture, new Vector2((float)Rand.NextDouble() * (1 + ReadySetGoPart) + GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ReadySetGo").Width / 2, (float)Rand.NextDouble() * (1 + ReadySetGoPart) + GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ReadySetGo").Width / 8), new Rectangle(0, ReadySetGoPart * (_resources.GetTexture("ReadySetGo").Height / 4), _resources.GetTexture("ReadySetGo").Width, _resources.GetTexture("ReadySetGo").Height / 4), Color.White);
            }
        }
        private void resetCharactersDrawnStatus()
        {
            Characters
                            .Where(character => character != null)
                            .ToList()
                            .ForEach(character => character.Drawn = false);
        }

        public void Close()
        {
            _resources.GetTexture("ReadySetGo").Unload();
            _resources.GetTexture("BattleBG").Unload();
        }

        public virtual void ReportKilling(int Killer, int Killee)
        {
            if (Characters[Killee].Lives == 0 && ThisGameType == GameType.Classic)
                ShortenBoard();

            if (Killer != -2 && Killer < Characters.Count)
            {
                Characters[Killer].Kills++;
            }
            ScoreKeeper.ReportKilling(Killer, Killee);

            if (ThisGameType == GameType.Spree && Killer != -2)
            {
                if (Characters[Killer].Kills > _spreeHighestKillCount)
                {
                    _spreeCurrentStep += Characters[Killer].Kills - _spreeHighestKillCount;
                    _spreeHighestKillCount = Characters[Killer].Kills;

                    if (_spreeCurrentStep >= _spreeStepSize)
                    {
                        _spreeCurrentStep -= _spreeStepSize;
                        if (Characters[Killer].Kills < _killsToWin && _stepsRemaining == 1)
                        {
                            // WHY IS THIS HAPPENING!?!??!?!
                        }
                        else
                        {
                            ShortenBoard();
                            int TimesShortened = 100 - _stepsRemaining;
                        }
                    }

                    if (Characters[Killer].Kills == _killsToWin)
                    {
                        _stepsRemaining = 1;
                        ShortenBoard();
                    }
                }
            }
        }

        public void RespawnChar(int x)
        {
            int newx = Rand.Next(0, GameGlobals.BOARD_WIDTH);
            int newy = Rand.Next(0, GameGlobals.BOARD_HEIGHT);

            while (Tiles[newx, newy].Dead || Tiles[newx, newy].CurrentTileCondition == TileCondition.RespawnPoint)
            {
                newx = Rand.Next(0, GameGlobals.BOARD_WIDTH);
                newy = Rand.Next(0, GameGlobals.BOARD_HEIGHT);
            }
            Vector2 newCharPos = InterpretCoordinates(new Vector2(newx, newy), false);
            Characters[x].Respawn(new Vector2(newCharPos.X + GameGlobals.TILE_SIZE / 2f, newCharPos.Y + GameGlobals.TILE_SIZE / 2f), new Vector2(newx, newy));
        }

        public void PauseGame(int playerindex)
        {
            _paused = true;
            BackgroundManager.ChangeBG(BackgroundType.Menu);

#if !ZUNE
            PausedPlayer = playerindex;
            if (PauseChoice)
            {
                PauseStrings[0] = DialogStrings.ContinueSelected;
                PauseStrings[1] = DialogStrings.Quit;
            }
            else
            {
                PauseStrings[0] = DialogStrings.Continue;
                PauseStrings[1] = DialogStrings.QuitSelected;
            }
#else
            SlaamGame.mainBlade.Status = BladeStatus.Out;
#endif
        }


        private static void markBoardOutline()
        {
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                // TODO FIX LOGIC!
                /*tiles[x, 0 + Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[x, 15 - Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[0 + Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[15 - Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);*/
            }
        }

        public virtual void EndGame()
        {
            ScoreKeeper.CalcTotals();
            for (int x = 0; x < Characters.Count; x++)
            {
                Characters[x].SaveProfileData();
            }
            ProfileManager.SaveProfiles();
            _screenDirector.ChangeTo(
                _statsScreenRequest.Resolve(new StatsScreenRequest(ScoreKeeper)));
        }
    }
}
