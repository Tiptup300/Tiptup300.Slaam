using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SlaamMono.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Audio;

#if ZUNE
using ZBlade;
#endif

// Xbox 360 - 18x12 45px
// ZUNE     - 10x07 30px

namespace SlaamMono
{
    public class GameScreen : IScreen
    {
        #region Variables

        public static GameScreen Instance;

        public GameScreenTimer Timer;
        public List<CharacterShell> SetupChars = new List<CharacterShell>();
        private readonly ILogger _logger;
        private readonly IMusicPlayer _musicPlayer;
        public List<Character> Characters = new List<Character>();
        public List<GameScreenScoreboard> Scoreboards = new List<GameScreenScoreboard>();
        public Tile[,] tiles = new Tile[GameGlobals.BOARD_WIDTH, GameGlobals.BOARD_HEIGHT];
        public Texture2D Tileset;
        public Vector2 Boardpos;

        //public Vector2 BoardposSav = new Vector2(240, 112);

        public Vector2 FinalBoardPosition
        {
            get
            {
                int width = (int)(SlaamGame.Graphics.PreferredBackBufferWidth/2f);
                int height = (int)(SlaamGame.Graphics.PreferredBackBufferHeight / 2f);
                int boardWidth = GameGlobals.BOARD_WIDTH * GameGlobals.TILE_SIZE;
                int boardHeight = GameGlobals.BOARD_HEIGHT * GameGlobals.TILE_SIZE;

                return new Vector2(width - boardWidth / 2f, height - boardHeight / 2f);
            }
        }

        public Random rand = new Random();
        private int Boardsize = 0;
        public int StepsRemaining;
        public GameStatus CurrentGameStatus = GameStatus.Waiting;
        public int ReadySetGoPart = 0;
        public Timer ReadySetGoThrottle = new Timer(new TimeSpan(0, 0, 0,0,325));
        public MatchScoreCollection ScoreKeeper;
        private bool Timing = false;
        public GameType ThisGameType;
        private bool Paused = false;
#if !ZUNE
        private bool PauseChoice = true;
        private int PausedPlayer = -1;
#endif
        private string[] PauseStrings = new string[2];
        private int KillsToWin = 0;
        public int NullChars = 0;

        private Timer PowerupTime = new Timer(new TimeSpan(0, 0, 0, 15));

        private float SpreeStepSize;
        private float SpreeCurrentStep;
        private int SpreeHighestKillCount;
        #endregion

        #region Constructor

        public GameScreen(List<CharacterShell> chars, ILogger logger, IMusicPlayer musicPlayer)
        {
            SetupChars = chars;
            _logger = logger;
            _musicPlayer = musicPlayer;

            ThisGameType = CurrentMatchSettings.GameType;
            SetupTheBoard(CurrentMatchSettings.BoardLocation);
            CurrentGameStatus = GameStatus.MovingBoard;

            Resources.ReadySetGo.Load();
            Resources.BattleBG.Load();
        }

        public void Initialize()
        {
            //Characters.Add(new Character(Texture2D.FromFile(Game1.Graphics.GraphicsDevice, "skins\\Tiptup300_link.png"), ProfileManager.Profiles[0], new Vector2(800, 800), InputComponent.Players[0], Color.Red));
            //Characters.Add(new Character(Texture2D.FromFile(Game1.Graphics.GraphicsDevice, "skins\\link901_Toadstool.png"), ProfileManager.Profiles[0], new Vector2(300, 300), InputComponent.Players[1], Color.Black));

            Boardpos = FinalBoardPosition;
            Boardpos.Y = -Tileset.Height;

            Timer = new GameScreenTimer(new Vector2(1024, 0),this);
            FeedManager.FeedsActive = false;
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                {
                    tiles[x, y] = new Tile(Boardpos, new Vector2(x, y), Tileset);
                }
            }
            ScoreKeeper = new MatchScoreCollection(this);
            ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.BattleScreen);
            _musicPlayer.Play(MusicTrack.Gameplay);


            if (ThisGameType == GameType.Classic)
                StepsRemaining = SetupChars.Count - 1;
            else if (ThisGameType == GameType.TimedSpree)
                StepsRemaining = 7;
            else if (ThisGameType == GameType.Spree)
            {
                //CurrentMatchSettings.KillsToWin = 7;
                StepsRemaining = 100;
                KillsToWin = CurrentMatchSettings.KillsToWin;
                SpreeStepSize = 10;
                SpreeCurrentStep = 0;
            }

#if ZUNE 
            SetupPauseMenu();
#endif
        }
#if ZUNE
        public MenuItemTree main = new MenuItemTree("");
        public void SetupPauseMenu()
        {
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;
            MenuTextItem resume = new MenuTextItem("Resume Game");
            resume.Activated += delegate {
                Paused = false;
                BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.BattleScreen);
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

        void quit_onSelected(object sender)
        {
            
        }

        void resume_onSelected(object sender)
        {
            
        }
#endif
        public virtual void SetupTheBoard(string BoardLoc)
        {
            Tileset = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + BoardLoc);//Texture2D.FromFile(Game1.Graphics.GraphicsDevice, BoardLoc);

            for (int x = 0; x < SetupChars.Count; x++)
            {
                if (SetupChars[x].Type == PlayerType.Player)
                {
                    Characters.Add(new Character(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupChars[x].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[x].SkinLocation)*/, SetupChars[x].CharProfile, new Vector2(-100, -100), InputComponent.Players[(int)SetupChars[x].PlayerIDX], SetupChars[x].PlayerColor, x));
                }
                else
                {
                    ProfileManager.AllProfiles[SetupChars[x].CharProfile].Skin = SetupChars[x].SkinLocation;
                    Characters.Add(new BotPlayer(SlaamGame.Content.Load<Texture2D>("content\\skins\\" + SetupChars[x].SkinLocation) /*Texture2D.FromFile(Game1.Graphics.GraphicsDevice, SetupChars[x].SkinLocation)*/, SetupChars[x].CharProfile, new Vector2(-100, -100), this, SetupChars[x].PlayerColor, Characters.Count));
                }
#if !ZUNE
                Scoreboards.Add(new GameScreenScoreboard(new Vector2(-250, 10 + x * Resources.GameScreenScoreBoard.Height), Characters[Characters.Count - 1], ThisGameType));
#else
                Scoreboards.Add(new GameScreenScoreboard(Vector2.Zero, Characters[Characters.Count - 1], ThisGameType));
#endif
            }
        }

        #endregion

        #region Update

        public virtual void Update()
        {
            //if (Input.GetKeyboard().PressedKey(Microsoft.Xna.Framework.Input.Keys.E))
              //  Characters[0].CurrentPowerup = new SlaamPowerup(this, Characters[0], 0);

            if (Paused)
            {
#if !ZUNE
                if (InputComponent.Players[0].PressedUp || InputComponent.Players[0].PressedDown)
                    PauseChoice = !PauseChoice;

                if (PauseChoice && InputComponent.Players[0].PressedAction)
                {
                    BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.BattleScreen);
                    Paused = false;
                }

                if (!PauseChoice && InputComponent.Players[0].PressedAction)
                    EndGame();

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
#endif
            }
            else
            {
                Timer.Update(Timing);
                for (int x = 0; x < Scoreboards.Count; x++)
                    Scoreboards[x].Update();

                if (CurrentGameStatus == GameStatus.MovingBoard)
                {
                    Boardpos.Y += FrameRateDirector.MovementFactor * (10f / 100f);

                    if (Boardpos.Y >= FinalBoardPosition.Y)
                    {
                        Boardpos = FinalBoardPosition;
                        CurrentGameStatus = GameStatus.Respawning;
                    }
                    for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                    {
                        for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                        {
                            tiles[x, y].ResetTileLoc(Boardpos, new Vector2(x, y));
                        }
                    }
                }
                else if (CurrentGameStatus == GameStatus.Respawning)
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
                else if (CurrentGameStatus == GameStatus.Waiting)
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
                            Timing = true;
                        }
                    }
                }
                else if (CurrentGameStatus == GameStatus.Playing)
                {
                    for (int x = 0; x < Characters.Count; x++)
                    {
                        if (Characters[x] != null)
                        {
                            int X1 = (int)(((Characters[x].Position.X - Boardpos.X) % GameGlobals.TILE_SIZE));
                            int Y1 = (int)(((Characters[x].Position.Y - Boardpos.Y) % GameGlobals.TILE_SIZE));
                            int X = (int)(((Characters[x].Position.X - Boardpos.X) - X1) / GameGlobals.TILE_SIZE);
                            int Y = (int)(((Characters[x].Position.Y - Boardpos.Y) - Y1) / GameGlobals.TILE_SIZE);
                            Characters[x].Update(tiles, new Vector2(X, Y), new Vector2(X1, Y1));
                            if (Characters[x].CurrentState == Character.CharacterState.Respawning)
                            {
                                RespawnChar(x);
                            }
                        }
                    }
                    for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                    {
                        for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                        {
                            tiles[x, y].Update();
                        }
                    }
                    PowerupTime.Update(FrameRateDirector.MovementFactorTimeSpan);
                    if (PowerupTime.Active)
                    {
                        bool found = true;
                        int newx = rand.Next(0, GameGlobals.BOARD_WIDTH);
                        int newy = rand.Next(0, GameGlobals.BOARD_HEIGHT);
                        int ct = 0;

                        while (tiles[newx,newy].CurrentTileCondition != Tile.TileCondition.Normal)
                        {
                            newx = rand.Next(0, GameGlobals.BOARD_WIDTH);
                            newy = rand.Next(0, GameGlobals.BOARD_HEIGHT);
                            ct++;
                            if (ct > 100)
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                        {
                            tiles[newx, newy].MarkWithPowerup(PowerupManager.GetRandomPowerup());
                        }
                    }
                }
                else if (CurrentGameStatus == GameStatus.Over)
                {
                    Timing = false;
                    ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
                    if (ReadySetGoThrottle.Active)
                        EndGame();
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch)
        {
            //batch.Draw(Resources.TileUnderlay, Boardpos, Color.White);
            if (Paused)
            {
#if !ZUNE
                batch.Draw(Resources.Dot, new Rectangle(0, 0, 1280, 1024), new Color(0, 0, 0, 100));
                batch.Draw(Resources.PauseScreen.Texture, new Vector2(640 - Resources.PauseScreen.Width / 2, 512 - Resources.PauseScreen.Height / 2), Color.White);
                Resources.DrawString(PauseStrings[0], new Vector2(640, 512 + 20), Resources.SegoeUIx32pt, FontAlignment.CompletelyCentered, Color.Black, false);
                Resources.DrawString(PauseStrings[1], new Vector2(640, 512 + 60), Resources.SegoeUIx32pt, FontAlignment.CompletelyCentered, Color.Black, false);
#else

#endif
                }
            else
            {
                /*for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                {
                    for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                    {
                        tiles[x, y].DrawShadow(batch);
                    }
                }*/

                for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                {
                    for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                    {
                        tiles[x, y].Draw(batch);
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

                for (int x = 0; x < Characters.Count; x++)
                    if(Characters[x] != null)
                        Characters[x].Drawn = false;

                //for (int x = 0; x < Scoreboards.Count; x++)
                    //Scoreboards[x].Draw(batch);

                if (CurrentGameStatus == GameStatus.Waiting || CurrentGameStatus == GameStatus.Over)
                {
                    batch.Draw(Resources.ReadySetGo.Texture, new Vector2((float)rand.NextDouble() * (1 + ReadySetGoPart) + (GameGlobals.DRAWING_GAME_WIDTH / 2) - Resources.ReadySetGo.Width / 2, (float)rand.NextDouble() * (1 + ReadySetGoPart) + GameGlobals.DRAWING_GAME_HEIGHT / 2 - Resources.ReadySetGo.Width / 8), new Rectangle(0, ReadySetGoPart * (Resources.ReadySetGo.Height / 4), Resources.ReadySetGo.Width, (Resources.ReadySetGo.Height / 4)), Color.White);
                }

                //Timer.Draw(batch);
            }
            
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            /*
            SetupChars = null;
            Characters = null;
            tiles = null;
            Tileset = null;
            Boardpos = Vector2.Zero;
            rand = null;
            Boardsize = -1;
            PlayersRemaining = -1;
             */

            Resources.ReadySetGo.Dispose();
            Resources.BattleBG.Dispose();
        }

        #endregion

        #region Extra Methods

        #region Killing Methods

        /// <summary>
        /// Update the kill tables for final scoring.
        /// </summary>
        /// <param name="Killer">Index of who killed.</param>
        /// <param name="Killee">Index of who was killed.</param>
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
                if (Characters[Killer].Kills > SpreeHighestKillCount)
                {
                    SpreeCurrentStep += Characters[Killer].Kills - SpreeHighestKillCount;
                    SpreeHighestKillCount = Characters[Killer].Kills;

                    if (SpreeCurrentStep >= SpreeStepSize)
                    {
                        SpreeCurrentStep -= SpreeStepSize;
                        if (Characters[Killer].Kills < KillsToWin && StepsRemaining == 1)
                        {
                            // WHY IS THIS HAPPENING!?!??!?!
                        }
                        else
                        {
                            ShortenBoard();
                            int TimesShortened = 100 - StepsRemaining;
                        }
                    }
                    
                    if (Characters[Killer].Kills == KillsToWin)
                    {
                        StepsRemaining = 1;
                        ShortenBoard();
                    }
                }
            }
        }

        /// <summary>
        /// Select a new location for a dead char. coming back and setup the tile for respawn.
        /// </summary>
        /// <param name="x">Character's Index</param>
        public void RespawnChar(int x)
        {
            int newx = rand.Next(0, GameGlobals.BOARD_WIDTH);
            int newy = rand.Next(0, GameGlobals.BOARD_HEIGHT);

            while (tiles[newx, newy].Dead || tiles[newx,newy].CurrentTileCondition == Tile.TileCondition.RespawnPoint)
            {
                newx = rand.Next(0, GameGlobals.BOARD_WIDTH);
                newy = rand.Next(0, GameGlobals.BOARD_HEIGHT);
            }
            Vector2 newCharPos = InterpretCoordinates(new Vector2(newx, newy), false);
            Characters[x].Respawn(new Vector2(newCharPos.X + (GameGlobals.TILE_SIZE / 2f), newCharPos.Y + (GameGlobals.TILE_SIZE / 2f)), new Vector2(newx, newy));
        }

        #endregion

        #region GameProgress Methods

        public void PauseGame(int playerindex)
        {
            Paused = true;
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);

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

        /// <summary>
        /// Marks the board outer edges for falling.
        /// </summary>
        public void ShortenBoard()
        {
            TimeSpan ShortenTime = new TimeSpan(0, 0, 0, 2);
            if (Boardsize < 6)
            {
                for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                {
                    // TODO FIX LOGIC!
                    /*tiles[x, 0 + Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                    tiles[x, 15 - Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                    tiles[0 + Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);
                    tiles[15 - Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);*/
                }

                Boardsize++;
            }
            StepsRemaining--;
            if (StepsRemaining == 0)
            {
                CurrentGameStatus = GameStatus.Over;
                ReadySetGoPart = 3;
                ReadySetGoThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
            }
        }

        /// <summary>
        /// Saves all profile information and goes to the stats screen.
        /// </summary>
        public virtual void EndGame()
        {
            ScoreKeeper.CalcTotals();
            for (int x = 0; x < Characters.Count; x++)
            {
                Characters[x].SaveProfileData();
            }
            ProfileManager.SaveProfiles();
            ScreenHelper.ChangeScreen(new StatsScreen(ScoreKeeper, _logger));
        }

        #endregion

        #region Coord Method

        /// <summary>
        /// Converts the real postion for the onboard positioning.
        /// </summary>
        /// <param name="pos">Coordinates to convert.</param>
        /// <param name="flip">Real->Board?</param>
        /// <returns></returns>
        public Vector2 InterpretCoordinates(Vector2 pos, bool flip)
        {
            if (!flip)
                return new Vector2(Boardpos.X + (pos.X * GameGlobals.TILE_SIZE), Boardpos.Y + (pos.Y * GameGlobals.TILE_SIZE));
            else
            {

                int X1 = (int)(((pos.X - Boardpos.X) % GameGlobals.TILE_SIZE));
                int Y1 = (int)(((pos.Y - Boardpos.Y) % GameGlobals.TILE_SIZE));
                int X = (int)(((pos.X - Boardpos.X) - X1) / GameGlobals.TILE_SIZE);
                int Y = (int)(((pos.Y - Boardpos.Y) - Y1) / GameGlobals.TILE_SIZE);

                if(pos.X < Boardpos.X)
                    X = -1;
                if(pos.Y < Boardpos.Y)
                    Y = -1;

                return new Vector2(X, Y);
            }
        }

        #endregion

        #endregion

        #region GameStatus Enum

        public enum GameStatus
        {
            MovingBoard,
            Respawning,
            Waiting,
            Playing,
            Over,
        }

        #endregion

    }
}
