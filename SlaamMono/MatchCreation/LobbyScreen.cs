using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.PlayerProfiles;
using SlaamMono.x_;
using System.Collections.Generic;
using System.IO;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreen : IScreen
    {
        public static Texture2D DefaultBoard;

        public Graph MainMenu;
        public List<CharacterShell> SetupCharacters;

        private const int MAX_PLAYERS = 4;

        private Texture2D CurrentBoardTexture;
        private int PlayerAmt;
        private string[] Dialogs;
        private string CurrentBoardLocation;
        private bool ViewingSettings;
        private IntRange MenuChoice;

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraphManager;
        private readonly IResolver<GameScreenRequest, GameScreen> _gameScreenRequest;

        public LobbyScreen(List<CharacterShell> chars, ILogger logger, IScreenManager screenDirector, PlayerColorResolver playerColorResolver, IResources resources, IRenderGraph renderGraphManager, IResolver<GameScreenRequest, GameScreen> gameScreenRequest)
        {
            SetupCharacters = chars;
            _logger = logger;
            _screenDirector = screenDirector;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _renderGraphManager = renderGraphManager;
            _gameScreenRequest = gameScreenRequest;

            initialize();
        }

        private void initialize()
        {
            Dialogs = new string[2];
            ViewingSettings = false;

            PlayerAmt = SetupCharacters.Count;
            MainMenu = new Graph(new Rectangle(10, 10, GameGlobals.DRAWING_GAME_WIDTH - 20, 624), 2, new Color(0, 0, 0, 150), _resources, _renderGraphManager);
            MainMenu.Items.Columns.Add("SETTING");
            MainMenu.Items.Columns.Add("SETTING");
            MainMenu.Items.Add(true,
                new GraphItemSetting(0, "GameType", "Classic", "Spree", "Timed Spree"),
                new GraphItemSetting(1, "Lives (Classic Only)", "3", "5", "10", "20", "40", "50", "100"),
                new GraphItemSetting(2, "Speed", "0.50x", "0.75x", "1.00x", "1.25x", "1.50x"),
                new GraphItemSetting(3, "Time of Match (Timed Spree Only)", "1 Minute", "2 Minutes", "3 Minutes", "5 Minutes", "10 Minutes", "20 Minutes", "30 Minutes", "45 Minutes", "1 Hour"),
                new GraphItemSetting(4, "Respawn Time", "Instant", "2 Seconds", "4 Seconds", "6 Seconds", "8 Seconds", "10 Seconds"),
                new GraphItemSetting(5, "Kills To Win (Spree Only)", "5", "10", "15", "20", "25", "30", "40", "50", "100"),
                new GraphItem("", "Choose Board..."),
                new GraphItem("", "Save"),
                new GraphItem("", "Cancel")
            );
            MenuChoice = new IntRange(0, 0, MainMenu.Items.Count - 1);
            CurrentMatchSettings.ReadValues(this);
            MainMenu.SetHighlight(MenuChoice.Value);

            if (CurrentMatchSettings.BoardLocation != null && CurrentMatchSettings.BoardLocation.Trim() != "" && File.Exists(CurrentMatchSettings.BoardLocation))
            {
                LoadBoard(CurrentMatchSettings.BoardLocation);
            }
            else
            {
                BoardThumbnailViewer viewer = new BoardThumbnailViewer(this, x_Di.Get<IResources>(), x_Di.Get<IScreenManager>());
                viewer.Open();
                while (!viewer.FoundBoard)
                {
                    viewer.Update();
                }
                LoadBoard(viewer.ValidBoard);
                viewer.Close();
            }

            SetupZune();
        }

        public static void SetupZune()
        {
            SlaamGame.mainBlade.Status = ZBlade.BladeStatus.In;
            SlaamGame.mainBlade.UserCanCloseMenu = false;

            ZBlade.InfoBlade.BladeHiddenSetup = ZBlade.InfoBlade.BladeInSetup;
            ZBlade.InfoBlade.BladeInSetup = new ZBlade.BladeSetup("Back", "Start", "Game Settings");
        }

        public static void ResetZune()
        {
            SlaamGame.mainBlade.Status = ZBlade.BladeStatus.Hidden;
            ZBlade.InfoBlade.BladeInSetup = ZBlade.InfoBlade.BladeHiddenSetup;
        }

        public static Texture2D LoadQuickBoard()
        {
            if (DefaultBoard == null)
            {
                BoardThumbnailViewer viewer = new BoardThumbnailViewer(null, x_Di.Get<IResources>(), x_Di.Get<IScreenManager>());
                viewer.Open();
                while (!viewer.FoundBoard)
                {
                    viewer.Update();
                }
                DefaultBoard = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + viewer.ValidBoard);
                viewer.Close();
            }

            return DefaultBoard;
        }

        public void Open()
        {
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            if (SetupCharacters.Count == 1)
            {
                AddComputer();
                PlayerAmt++;
            }
            FeedManager.InitializeFeeds(DialogStrings.LobbyScreenFeed);
        }

        public void Update()
        {
            BackgroundManager.SetRotation(1f);
            if (ViewingSettings)
            {
                if (InputComponent.Players[0].PressedDown)
                {
                    MenuChoice.Add(1);
                    MainMenu.SetHighlight(MenuChoice.Value);
                }
                if (InputComponent.Players[0].PressedUp)
                {
                    MenuChoice.Sub(1);
                    MainMenu.SetHighlight(MenuChoice.Value);
                }

                if (MainMenu.Items[MenuChoice.Value].GetType() == typeof(GraphItemSetting))
                {
                    if (InputComponent.Players[0].PressedLeft)
                    {
                        MainMenu.Items[MenuChoice.Value].ToSetting().ChangeValue(false);
                        MainMenu.CalculateBlocks();
                    }
                    else if (InputComponent.Players[0].PressedRight)
                    {
                        MainMenu.Items[MenuChoice.Value].ToSetting().ChangeValue(true);
                        MainMenu.CalculateBlocks();
                    }
                }
                else
                {
                    if (InputComponent.Players[0].PressedAction)
                    {
                        if (MainMenu.Items[MenuChoice.Value].Details[1] == "Save")
                        {
                            CurrentMatchSettings.SaveValues(this, CurrentBoardLocation);
                            ViewingSettings = false;
                            SetupZune();
                        }
                        else if (MainMenu.Items[MenuChoice.Value].Details[1] == "Cancel")
                        {
                            CurrentMatchSettings.ReadValues(this);
                            ViewingSettings = false;
                            SetupZune();
                        }
                        else
                        {
                            _screenDirector.ChangeTo(new BoardThumbnailViewer(this, x_Di.Get<IResources>(), x_Di.Get<IScreenManager>()));
                        }
                        BackgroundManager.ChangeBG(BackgroundType.Menu);
                    }
                }
            }
            else
            {
                if (InputComponent.Players[0].PressedAction2)
                {
                    _screenDirector.ChangeTo(
                        new ClassicCharSelectScreen(
                            x_Di.Get<ILogger>(),
                            x_Di.Get<IScreenManager>()));
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedUp && SetupCharacters.Count < MAX_PLAYERS)
                {
                    AddComputer();
                }
                if (InputComponent.Players[0].PressedDown && SetupCharacters.Count > PlayerAmt)
                {
                    ProfileManager.ResetBot(SetupCharacters[SetupCharacters.Count - 1].CharProfile);
                    SetupCharacters.RemoveAt(SetupCharacters.Count - 1);
                }

                if (InputComponent.Players[0].PressedStart)
                {
                    CurrentMatchSettings.SaveValues(this, CurrentBoardLocation);
                    GameScreen.Instance = _gameScreenRequest.Resolve(new GameScreenRequest(SetupCharacters));
                    _screenDirector.ChangeTo(GameScreen.Instance);
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedAction)
                {
                    ViewingSettings = true;
                    ResetZune();

                    BackgroundManager.ChangeBG(BackgroundType.Normal);
                }

            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (ViewingSettings)
            {
                MainMenu.Draw(batch);
            }
            else
            {
                batch.Draw(_resources.GetTexture("LobbyUnderlay").Texture, Vector2.Zero, Color.White);
                float YOffset = 75;

                for (int x = 0; x < SetupCharacters.Count; x++)
                {
                    batch.Draw(_resources.GetTexture("LobbyCharBar").Texture, new Vector2(0, YOffset + 30 * x), Color.White);
                    batch.Draw(_resources.GetTexture("LobbyColorPreview").Texture, new Vector2(0, YOffset + 30 * x), SetupCharacters[x].PlayerColor);
                    if (SetupCharacters[x].Type == PlayerType.Player)
                        RenderGraph.Instance.RenderText(DialogStrings.Player + (x + 1) + ": " + ProfileManager.AllProfiles[SetupCharacters[x].CharProfile].Name, new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
                    else
                        RenderGraph.Instance.RenderText(DialogStrings.Player + (x + 1) + ": *" + ProfileManager.AllProfiles[SetupCharacters[x].CharProfile].Name + "*", new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Red, Alignment.TopLeft, false);
                }
                batch.Draw(_resources.GetTexture("LobbyOverlay").Texture, Vector2.Zero, Color.White);
            }
        }

        public void Close()
        {
            CurrentBoardTexture = null;
            _resources.GetTexture("LobbyUnderlay").Dispose();
            _resources.GetTexture("LobbyCharBar").Dispose();
            _resources.GetTexture("LobbyColorPreview").Dispose();
            _resources.GetTexture("LobbyOverlay").Dispose();
        }

        /// <summary>
        /// Loads the new Board Texture and loads its name/creator.
        /// </summary>
        public void LoadBoard(string brdloc)
        {
            if (CurrentBoardTexture != null && CurrentBoardLocation == brdloc)
            {

            }
            else
            {
                try
                {
                    CurrentBoardTexture = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + brdloc);
                    CurrentBoardLocation = brdloc;
                }
                catch (FileNotFoundException)
                {

                }
                Dialogs[0] = DialogStrings.CurrentBoard + CurrentBoardLocation.Substring(CurrentBoardLocation.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\", "");
                if (CurrentBoardLocation.IndexOf('_') >= 0)
                    Dialogs[1] = DialogStrings.CreatedBy + CurrentBoardLocation.Substring(0, CurrentBoardLocation.IndexOf('_')).Replace(".png", "").Replace("boards\\", "");
                else
                    Dialogs[1] = "";

                DefaultBoard = CurrentBoardTexture;
            }
        }

        /// <summary>
        /// Adds a new computer player.
        /// </summary>
        private void AddComputer()
        {
            SetupCharacters.Add(new CharacterShell(ClassicCharSelectScreen.ReturnRandSkin(_logger), ProfileManager.GetBotProfile(), (ExtendedPlayerIndex)SetupCharacters.Count, PlayerType.Computer, _playerColorResolver.GetColorByIndex(SetupCharacters.Count)));
        }
    }
}
