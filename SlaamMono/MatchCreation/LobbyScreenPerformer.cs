using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
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
using System;
using System.IO;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreenPerformer : IStatePerformer
    {
        private static Texture2D _defaultBoard;

        private const int _maxPlayers = 4;

        private LobbyScreenState _state = new LobbyScreenState();

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraphManager;
        private readonly IResolver<GameScreenRequestState, GameScreenPerformer> _gameScreenRequest;
        private readonly IResolver<BoardSelectionScreenRequestState, BoardSelectionScreenPerformer> _boardSelectionScreenResolver;
        private readonly IResolver<CharacterSelectionScreenRequestState, CharacterSelectionScreenPerformer> _characterSelectionScreenResolver;

        public LobbyScreenPerformer(
            ILogger logger,
            IScreenManager screenDirector,
            PlayerColorResolver playerColorResolver,
            IResources resources,
            IRenderGraph renderGraphManager,
            IResolver<GameScreenRequestState, GameScreenPerformer> gameScreenRequest,
            IResolver<BoardSelectionScreenRequestState, BoardSelectionScreenPerformer> boardSelectionScreenResolver,
            IResolver<CharacterSelectionScreenRequestState, CharacterSelectionScreenPerformer> characterSelectionScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _renderGraphManager = renderGraphManager;
            _gameScreenRequest = gameScreenRequest;
            _boardSelectionScreenResolver = boardSelectionScreenResolver;
            _characterSelectionScreenResolver = characterSelectionScreenResolver;
        }


        public void Initialize(LobbyScreenRequestState request)
        {
            _state.SetupCharacters = request.CharacterShells;
        }

        public void InitializeState()
        {
            _state.Dialogs = new string[2];
            _state.ViewingSettings = false;

            _state.PlayerAmt = _state.SetupCharacters.Count;
            _state.MainMenu = new Graph(new Rectangle(10, 10, GameGlobals.DRAWING_GAME_WIDTH - 20, 624), 2, new Color(0, 0, 0, 150), _resources, _renderGraphManager);
            _state.MainMenu.Items.Columns.Add("SETTING");
            _state.MainMenu.Items.Columns.Add("SETTING");
            _state.MainMenu.Items.Add(true,
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
            _state.MenuChoice = new IntRange(0, 0, _state.MainMenu.Items.Count - 1);
            readMatchSettingsFromFile();
            MatchSettings.CurrentMatchSettings = buildMatchSettings();
            _state.MainMenu.SetHighlight(_state.MenuChoice.Value);

            if (MatchSettings.CurrentMatchSettings.BoardLocation != null && MatchSettings.CurrentMatchSettings.BoardLocation.Trim() != "" && File.Exists(MatchSettings.CurrentMatchSettings.BoardLocation))
            {
                LoadBoard(MatchSettings.CurrentMatchSettings.BoardLocation);
            }
            else
            {
                BoardSelectionScreenPerformer viewer = _boardSelectionScreenResolver.Resolve(new BoardSelectionScreenRequestState(this));
                viewer.InitializeState();
                while (!viewer.x_HasFoundBoard)
                {
                    viewer.Perform();
                }
                LoadBoard(viewer.x_IsValidBoard);
                viewer.Close();
            }

            SetupZune();
            if (_state.SetupCharacters.Count == 1)
            {
                AddComputer();
                _state.PlayerAmt++;
            }
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
                BoardSelectionScreenPerformer viewer = null;//= new BoardSelectionScreen(null, null);
                new BoardSelectionScreenRequestState(null);
                viewer.InitializeState();
                while (!viewer.x_HasFoundBoard)
                {
                    viewer.Perform();
                }
                _defaultBoard = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + viewer.x_IsValidBoard);
                viewer.Close();
            }

            return _defaultBoard;
        }

        public IState Perform()
        {
            if (_state.ViewingSettings)
            {
                if (InputComponent.Players[0].PressedDown)
                {
                    _state.MenuChoice.Add(1);
                    _state.MainMenu.SetHighlight(_state.MenuChoice.Value);
                }
                if (InputComponent.Players[0].PressedUp)
                {
                    _state.MenuChoice.Sub(1);
                    _state.MainMenu.SetHighlight(_state.MenuChoice.Value);
                }

                if (_state.MainMenu.Items[_state.MenuChoice.Value].GetType() == typeof(GraphItemSetting))
                {
                    if (InputComponent.Players[0].PressedLeft)
                    {
                        _state.MainMenu.Items[_state.MenuChoice.Value].ToSetting().ChangeValue(false);
                        _state.MainMenu.CalculateBlocks();
                    }
                    else if (InputComponent.Players[0].PressedRight)
                    {
                        _state.MainMenu.Items[_state.MenuChoice.Value].ToSetting().ChangeValue(true);
                        _state.MainMenu.CalculateBlocks();
                    }
                }
                else
                {
                    if (InputComponent.Players[0].PressedAction)
                    {
                        if (_state.MainMenu.Items[_state.MenuChoice.Value].Details[1] == "Save")
                        {
                            MatchSettings.CurrentMatchSettings = buildMatchSettings();
                            writeMatchSettingsToFile();
                            _state.ViewingSettings = false;
                            SetupZune();
                        }
                        else if (_state.MainMenu.Items[_state.MenuChoice.Value].Details[1] == "Cancel")
                        {
                            readMatchSettingsFromFile();
                            MatchSettings.CurrentMatchSettings = buildMatchSettings();
                            _state.ViewingSettings = false;
                            SetupZune();
                        }
                        else
                        {
                            _screenDirector.ChangeTo(_boardSelectionScreenResolver.Resolve(new BoardSelectionScreenRequestState(this)));
                        }

                    }
                }
            }
            else
            {
                if (InputComponent.Players[0].PressedAction2)
                {
                    _screenDirector.ChangeTo(_characterSelectionScreenResolver.Resolve(new CharacterSelectionScreenRequestState()));
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedUp && _state.SetupCharacters.Count < _maxPlayers)
                {
                    AddComputer();
                }
                if (InputComponent.Players[0].PressedDown && _state.SetupCharacters.Count > _state.PlayerAmt)
                {
                    ProfileManager.ResetBot(_state.SetupCharacters[_state.SetupCharacters.Count - 1].CharacterProfileIndex);
                    _state.SetupCharacters.RemoveAt(_state.SetupCharacters.Count - 1);
                }

                if (InputComponent.Players[0].PressedStart)
                {
                    MatchSettings matchSettings = buildMatchSettings();
                    MatchSettings.CurrentMatchSettings = matchSettings;
                    writeMatchSettingsToFile();
                    _screenDirector.ChangeTo(_gameScreenRequest.Resolve(new GameScreenRequestState(_state.SetupCharacters, matchSettings)));
                    ProfileManager.ResetAllBots();
                    ResetZune();
                }

                if (InputComponent.Players[0].PressedAction)
                {
                    _state.ViewingSettings = true;
                    ResetZune();


                }

            }
            return _state;
        }

        private MatchSettings buildMatchSettings()
        {
            MatchSettings output;

            output = new MatchSettings();
            switch (_state.MainMenu.Items[0].ToSetting().OptionChoice.Value)
            {
                case 0: output.GameType = GameType.Classic; break;
                case 1: output.GameType = GameType.Spree; break;
                case 2: output.GameType = GameType.TimedSpree; break;
            }
            switch (_state.MainMenu.Items[1].ToSetting().OptionChoice.Value)
            {
                case 0: output.LivesAmt = 3; break;
                case 1: output.LivesAmt = 5; break;
                case 2: output.LivesAmt = 10; break;
                case 3: output.LivesAmt = 20; break;
                case 4: output.LivesAmt = 40; break;
                case 5: output.LivesAmt = 50; break;
                case 6: output.LivesAmt = 100; break;
            }

            switch (_state.MainMenu.Items[2].ToSetting().OptionChoice.Value)
            {
                case 0: output.SpeedMultiplyer = .5f; break;
                case 1: output.SpeedMultiplyer = .75f; break;
                case 2: output.SpeedMultiplyer = 1f; break;
                case 3: output.SpeedMultiplyer = 1.25f; break;
                case 4: output.SpeedMultiplyer = 1.5f; break;
            }

            switch (_state.MainMenu.Items[3].ToSetting().OptionChoice.Value)
            {
                case 0: output.TimeOfMatch = new TimeSpan(0, 1, 0); break;
                case 1: output.TimeOfMatch = new TimeSpan(0, 2, 0); break;
                case 2: output.TimeOfMatch = new TimeSpan(0, 3, 0); break;
                case 3: output.TimeOfMatch = new TimeSpan(0, 5, 0); break;
                case 4: output.TimeOfMatch = new TimeSpan(0, 10, 0); break;
                case 5: output.TimeOfMatch = new TimeSpan(0, 20, 0); break;
                case 6: output.TimeOfMatch = new TimeSpan(0, 40, 0); break;
                case 7: output.TimeOfMatch = new TimeSpan(0, 45, 0); break;
                case 8: output.TimeOfMatch = new TimeSpan(0, 60, 0); break;
            }

            switch (_state.MainMenu.Items[4].ToSetting().OptionChoice.Value)
            {
                case 0: output.RespawnTime = new TimeSpan(0, 0, 0); break;
                case 1: output.RespawnTime = new TimeSpan(0, 0, 2); break;
                case 2: output.RespawnTime = new TimeSpan(0, 0, 4); break;
                case 3: output.RespawnTime = new TimeSpan(0, 0, 6); break;
                case 4: output.RespawnTime = new TimeSpan(0, 0, 8); break;
                case 5: output.RespawnTime = new TimeSpan(0, 0, 10); break;
            }

            switch (_state.MainMenu.Items[5].ToSetting().OptionChoice.Value)
            {
                case 0: output.KillsToWin = 5; break;
                case 1: output.KillsToWin = 10; break;
                case 2: output.KillsToWin = 15; break;
                case 3: output.KillsToWin = 20; break;
                case 4: output.KillsToWin = 25; break;
                case 5: output.KillsToWin = 30; break;
                case 6: output.KillsToWin = 40; break;
                case 7: output.KillsToWin = 50; break;
                case 8: output.KillsToWin = 100; break;

            }
            output.BoardLocation = _state.BoardLocation;

            return output;
        }

        private void writeMatchSettingsToFile()
        {
            XnaContentWriter writer = new XnaContentWriter(x_Di.Get<ProfileFileVersion>());
            writer.Initialize(DialogStrings.MatchSettingsFilename);

            for (int x = 0; x < 6; x++)
            {
                int y = _state.MainMenu.Items[x].ToSetting().OptionChoice.Value;
                writer.Write(y);
            }
            writer.Write(_state.BoardLocation != null ? _state.BoardLocation : "");

            writer.Close();
        }

        public void readMatchSettingsFromFile()
        {
            XnaContentReader reader;

            reader = new XnaContentReader(_logger, x_Di.Get<ProfileFileVersion>());
            try
            {

                reader.Initialize(DialogStrings.MatchSettingsFilename);

                if (reader.IsWrongVersion())
                {
                    MatchSettings.CurrentMatchSettings = buildMatchSettings();
                    throw new EndOfStreamException();
                }

                for (int x = 0; x < 6; x++)
                {
                    int y = reader.ReadInt32();
                    _state.MainMenu.Items[x].ToSetting().ChangeValue(y);
                }

                _state.BoardLocation = reader.ReadString();
            }
            catch (EndOfStreamException)
            {
            }
            finally
            {
                reader.Close();
                MatchSettings.CurrentMatchSettings = buildMatchSettings();
            }
        }

        public void RenderState(SpriteBatch batch)
        {
            if (_state.ViewingSettings)
            {
                _state.MainMenu.Draw(batch);
            }
            else
            {
                batch.Draw(_resources.GetTexture("LobbyUnderlay").Texture, Vector2.Zero, Color.White);
                float YOffset = 75;

                for (int x = 0; x < _state.SetupCharacters.Count; x++)
                {
                    batch.Draw(_resources.GetTexture("LobbyCharBar").Texture, new Vector2(0, YOffset + 30 * x), Color.White);
                    batch.Draw(_resources.GetTexture("LobbyColorPreview").Texture, new Vector2(0, YOffset + 30 * x), _state.SetupCharacters[x].PlayerColor);
                    if (_state.SetupCharacters[x].PlayerType == PlayerType.Player)
                        RenderGraph.Instance.RenderText(DialogStrings.Player + (x + 1) + ": " + ProfileManager.AllProfiles[_state.SetupCharacters[x].CharacterProfileIndex].Name, new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
                    else
                        RenderGraph.Instance.RenderText(DialogStrings.Player + (x + 1) + ": *" + ProfileManager.AllProfiles[_state.SetupCharacters[x].CharacterProfileIndex].Name + "*", new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Red, Alignment.TopLeft, false);
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
            if (_state.CurrentBoardTexture != null && _state.BoardLocation == boardLocation)
            {
                return;
            }

            try
            {
                _state.CurrentBoardTexture = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + boardLocation);
                _state.BoardLocation = boardLocation;
            }
            catch (FileNotFoundException)
            {

            }
            _state.Dialogs[0] = DialogStrings.CurrentBoard + _state.BoardLocation.Substring(_state.BoardLocation.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\", "");
            if (_state.BoardLocation.IndexOf('_') >= 0)
            {
                _state.Dialogs[1] = DialogStrings.CreatedBy + _state.BoardLocation.Substring(0, _state.BoardLocation.IndexOf('_')).Replace(".png", "").Replace("boards\\", "");
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
            _state.SetupCharacters.Add(
                new CharacterShell(
                    skinLocation: SkinLoadingFunctions.ReturnRandSkin(_logger),
                    characterProfileIndex: ProfileManager.GetBotProfile(),
                    playerIndex: (ExtendedPlayerIndex)_state.SetupCharacters.Count,
                    playerType: PlayerType.Computer,
                    playerColor: _playerColorResolver.GetColorByIndex(_state.SetupCharacters.Count)));
        }
    }
}
