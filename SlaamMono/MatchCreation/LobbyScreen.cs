using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Graph MainMenu;
        public List<CharacterShell> SetupCharacters;


        private static Texture2D _defaultBoard;

        private const int MAX_PLAYERS = 4;

        private LobbyScreenState _state = new LobbyScreenState();


        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraphManager;
        private readonly IResolver<GameScreenRequest, GameScreen> _gameScreenRequest;
        private readonly IResolver<BoardSelectionScreenRequest, BoardSelectionScreen> _boardSelectionScreenResolver;
        private readonly IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen> _characterSelectionScreenResolver;

        public LobbyScreen(
            List<CharacterShell> chars,
            ILogger logger,
            IScreenManager screenDirector,
            PlayerColorResolver playerColorResolver,
            IResources resources,
            IRenderGraph renderGraphManager,
            IResolver<GameScreenRequest, GameScreen> gameScreenRequest,
            IResolver<BoardSelectionScreenRequest, BoardSelectionScreen> boardSelectionScreenResolver,
            IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen> characterSelectionScreenResolver)
        {
            SetupCharacters = chars;
            _logger = logger;
            _screenDirector = screenDirector;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _renderGraphManager = renderGraphManager;
            _gameScreenRequest = gameScreenRequest;
            _boardSelectionScreenResolver = boardSelectionScreenResolver;
            _characterSelectionScreenResolver = characterSelectionScreenResolver;
            initialize();
        }

        private void initialize()
        {
            _state.Dialogs = new string[2];
            _state.ViewingSettings = false;

            _state.PlayerAmt = SetupCharacters.Count;
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
            _state.MenuChoice = new IntRange(0, 0, MainMenu.Items.Count - 1);
            CurrentMatchSettings.ReadValues(this);
            MainMenu.SetHighlight(_state.MenuChoice.Value);

            if (CurrentMatchSettings.BoardLocation != null && CurrentMatchSettings.BoardLocation.Trim() != "" && File.Exists(CurrentMatchSettings.BoardLocation))
            {
                LoadBoard(CurrentMatchSettings.BoardLocation);
            }
            else
            {
                BoardSelectionScreen viewer = _boardSelectionScreenResolver.Resolve(new BoardSelectionScreenRequest(this));
                viewer.Open();
                while (!viewer.HasFoundBoard)
                {
                    viewer.Update();
                }
                LoadBoard(viewer.IsValidBoard);
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
            if (_defaultBoard == null)
            {
                // todo: this will need fixed.
                BoardSelectionScreen viewer = new BoardSelectionScreen(null, null);
                viewer.Open();
                while (!viewer.HasFoundBoard)
                {
                    viewer.Update();
                }
                _defaultBoard = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + viewer.IsValidBoard);
                viewer.Close();
            }

            return _defaultBoard;
        }

        public void Open()
        {
            BackgroundManager.ChangeBG(BackgroundType.Menu);
            if (SetupCharacters.Count == 1)
            {
                AddComputer();
                _state.PlayerAmt++;
            }
            FeedManager.InitializeFeeds(DialogStrings.LobbyScreenFeed);
        }

        public void Update()
        {
            BackgroundManager.SetRotation(1f);
            if (_state.ViewingSettings)
            {
                if (InputComponent.Players[0].PressedDown)
                {
                    _state.MenuChoice.Add(1);
                    MainMenu.SetHighlight(_state.MenuChoice.Value);
                }
                if (InputComponent.Players[0].PressedUp)
                {
                    _state.MenuChoice.Sub(1);
                    MainMenu.SetHighlight(_state.MenuChoice.Value);
                }

                if (MainMenu.Items[_state.MenuChoice.Value].GetType() == typeof(GraphItemSetting))
                {
                    if (InputComponent.Players[0].PressedLeft)
                    {
                        MainMenu.Items[_state.MenuChoice.Value].ToSetting().ChangeValue(false);
                        MainMenu.CalculateBlocks();
                    }
                    else if (InputComponent.Players[0].PressedRight)
                    {
                        MainMenu.Items[_state.MenuChoice.Value].ToSetting().ChangeValue(true);
                        MainMenu.CalculateBlocks();
                    }
                }
                else
                {
                    if (InputComponent.Players[0].PressedAction)
                    {
                        if (MainMenu.Items[_state.MenuChoice.Value].Details[1] == "Save")
                        {
                            CurrentMatchSettings.SaveValues(this, _state.CurrentBoardLocation);
                            _state.ViewingSettings = false;
                            SetupZune();
                        }
                        else if (MainMenu.Items[_state.MenuChoice.Value].Details[1] == "Cancel")
                        {
                            CurrentMatchSettings.ReadValues(this);
                            _state.ViewingSettings = false;
                            SetupZune();
                        }
                        else
                        {
                            _screenDirector.ChangeTo(_boardSelectionScreenResolver.Resolve(new BoardSelectionScreenRequest(this)));
                        }
                        BackgroundManager.ChangeBG(BackgroundType.Menu);
                    }
                }
            }
            else
            {
                if (InputComponent.Players[0].PressedAction2)
                {
                    _screenDirector.ChangeTo(_characterSelectionScreenResolver.Resolve(new CharacterSelectionScreenRequest()));
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedUp && SetupCharacters.Count < MAX_PLAYERS)
                {
                    AddComputer();
                }
                if (InputComponent.Players[0].PressedDown && SetupCharacters.Count > _state.PlayerAmt)
                {
                    ProfileManager.ResetBot(SetupCharacters[SetupCharacters.Count - 1].CharProfile);
                    SetupCharacters.RemoveAt(SetupCharacters.Count - 1);
                }

                if (InputComponent.Players[0].PressedStart)
                {
                    CurrentMatchSettings.SaveValues(this, _state.CurrentBoardLocation);
                    GameScreen.Instance = _gameScreenRequest.Resolve(new GameScreenRequest(SetupCharacters));
                    _screenDirector.ChangeTo(GameScreen.Instance);
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedAction)
                {
                    _state.ViewingSettings = true;
                    ResetZune();

                    BackgroundManager.ChangeBG(BackgroundType.Normal);
                }

            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (_state.ViewingSettings)
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
            _state.CurrentBoardTexture = null;
            _resources.GetTexture("LobbyUnderlay").Unload();
            _resources.GetTexture("LobbyCharBar").Unload();
            _resources.GetTexture("LobbyColorPreview").Unload();
            _resources.GetTexture("LobbyOverlay").Unload();
        }

        /// <summary>
        /// Loads the new Board Texture and loads its name/creator.
        /// </summary>
        public void LoadBoard(string boardLocation)
        {
            if (_state.CurrentBoardTexture != null && _state.CurrentBoardLocation == boardLocation)
            {
                return;
            }

            try
            {
                _state.CurrentBoardTexture = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + boardLocation);
                _state.CurrentBoardLocation = boardLocation;
            }
            catch (FileNotFoundException)
            {

            }
            _state.Dialogs[0] = DialogStrings.CurrentBoard + _state.CurrentBoardLocation.Substring(_state.CurrentBoardLocation.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\", "");
            if (_state.CurrentBoardLocation.IndexOf('_') >= 0)
            {
                _state.Dialogs[1] = DialogStrings.CreatedBy + _state.CurrentBoardLocation.Substring(0, _state.CurrentBoardLocation.IndexOf('_')).Replace(".png", "").Replace("boards\\", "");
            }
            else
            {
                _state.Dialogs[1] = "";
            }

            _defaultBoard = _state.CurrentBoardTexture;

        }

        /// <summary>
        /// Adds a new computer player.
        /// </summary>
        private void AddComputer()
        {
            SetupCharacters.Add(new CharacterShell(CharacterSelectionScreen.ReturnRandSkin(_logger), ProfileManager.GetBotProfile(), (ExtendedPlayerIndex)SetupCharacters.Count, PlayerType.Computer, _playerColorResolver.GetColorByIndex(SetupCharacters.Count)));
        }
    }
}
