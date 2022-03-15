using Microsoft.Xna.Framework;
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
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class LobbyScreenPerformer : IPerformer<LobbyScreenState>
    {
        private const int _maxPlayers = 4;

        private LobbyScreenState _state = new LobbyScreenState();

        private readonly ILogger _logger;
        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;
        private readonly IRenderService _renderService;
        private readonly IInputService _inputService;

        public LobbyScreenPerformer(
            ILogger logger,
            PlayerColorResolver playerColorResolver,
            IResources resources,
            IRenderService renderGraphManager,
            IInputService inputService)
        {
            _logger = logger;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _renderService = renderGraphManager;
            _inputService = inputService;
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
            _state.MainMenu = new Graph(new Rectangle(10, 10, GameGlobals.DRAWING_GAME_WIDTH - 20, 624), 2, new Color(0, 0, 0, 150), _resources, _renderService);
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
                LobbyScreenFunctions.LoadBoard(MatchSettings.CurrentMatchSettings.BoardLocation, _state);
            }
            else
            {
                // This will need to be redone to accomidate the State/Logic system.
                // this should have been done in the request step.... I think....

                //BoardSelectionScreenPerformer viewer = _boardSelectionScreenResolver.Resolve(new BoardSelectionScreenRequestState(_state));
                //viewer.InitializeState();
                //while (!viewer.x_HasFoundBoard)
                //{
                //    viewer.Perform();
                //}
                //LobbyScreenFunctions.LoadBoard(viewer.x_IsValidBoard, _state);
                //viewer.Close();
            }

            LobbyScreenFunctions.SetupZune();
            if (_state.SetupCharacters.Count == 1)
            {
                addComputer();
                _state.PlayerAmt++;
            }
        }
        private void addComputer()
        {
            _state.SetupCharacters.Add(
                new CharacterShell(
                    skinLocation: SkinLoadingFunctions.ReturnRandSkin(_logger),
                    characterProfileIndex: ProfileManager.GetBotProfile(),
                    playerIndex: (ExtendedPlayerIndex)_state.SetupCharacters.Count,
                    playerType: PlayerType.Computer,
                    playerColor: _playerColorResolver.GetColorByIndex(_state.SetupCharacters.Count)));
        }

        public IState Perform(LobbyScreenState state)
        {

            if (state.ViewingSettings)
            {
                if (_inputService.GetPlayers()[0].PressedDown)
                {
                    state.MenuChoice.Add(1);
                    state.MainMenu.SetHighlight(state.MenuChoice.Value);
                }
                if (_inputService.GetPlayers()[0].PressedUp)
                {
                    state.MenuChoice.Sub(1);
                    state.MainMenu.SetHighlight(state.MenuChoice.Value);
                }

                if (state.MainMenu.Items[state.MenuChoice.Value].GetType() == typeof(GraphItemSetting))
                {
                    if (_inputService.GetPlayers()[0].PressedLeft)
                    {
                        state.MainMenu.Items[state.MenuChoice.Value].ToSetting().ChangeValue(false);
                        state.MainMenu.CalculateBlocks();
                    }
                    else if (_inputService.GetPlayers()[0].PressedRight)
                    {
                        state.MainMenu.Items[state.MenuChoice.Value].ToSetting().ChangeValue(true);
                        state.MainMenu.CalculateBlocks();
                    }
                }
                else
                {
                    if (_inputService.GetPlayers()[0].PressedAction)
                    {
                        if (state.MainMenu.Items[state.MenuChoice.Value].Details[1] == "Save")
                        {
                            MatchSettings.CurrentMatchSettings = buildMatchSettings();
                            writeMatchSettingsToFile();
                            state.ViewingSettings = false;
                            LobbyScreenFunctions.SetupZune();
                        }
                        else if (state.MainMenu.Items[state.MenuChoice.Value].Details[1] == "Cancel")
                        {
                            readMatchSettingsFromFile();
                            MatchSettings.CurrentMatchSettings = buildMatchSettings();
                            state.ViewingSettings = false;
                            LobbyScreenFunctions.SetupZune();
                        }
                        else
                        {
                            // @State/Logic - this will need to be changed to a Request -> State resolver.
                            return new BoardSelectionScreenRequestState(state);
                        }
                    }
                }
            }
            else
            {
                if (_inputService.GetPlayers()[0].PressedAction2)
                {
                    ProfileManager.ResetAllBots();
                    LobbyScreenFunctions.SetupZune();
                    // @State/Logic - this will need to be changed to a Requaest -> State resolver.
                    return new CharacterSelectionScreenRequestState();
                }

                if (_inputService.GetPlayers()[0].PressedUp && state.SetupCharacters.Count < _maxPlayers)
                {
                    addComputer();
                }
                if (_inputService.GetPlayers()[0].PressedDown && state.SetupCharacters.Count > state.PlayerAmt)
                {
                    ProfileManager.ResetBot(state.SetupCharacters[state.SetupCharacters.Count - 1].CharacterProfileIndex);
                    state.SetupCharacters.RemoveAt(state.SetupCharacters.Count - 1);
                }

                if (_inputService.GetPlayers()[0].PressedStart)
                {
                    MatchSettings matchSettings = buildMatchSettings();
                    MatchSettings.CurrentMatchSettings = matchSettings;
                    writeMatchSettingsToFile();
                    ProfileManager.ResetAllBots();
                    LobbyScreenFunctions.SetupZune();
                    // @State/Logic - this will need to be changed to a Request -> State resolver.
                    return new GameScreenRequestState(state.SetupCharacters, matchSettings);
                }

                if (_inputService.GetPlayers()[0].PressedAction)
                {
                    state.ViewingSettings = true;
                    LobbyScreenFunctions.SetupZune();
                }

            }
            return state;
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
        private void readMatchSettingsFromFile()
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

        public void Render(LobbyScreenState state)
        {
            _renderService.Render(batch =>
            {
                if (state.ViewingSettings)
                {
                    state.MainMenu.Draw(batch);
                }
                else
                {
                    batch.Draw(_resources.GetTexture("LobbyUnderlay").Texture, Vector2.Zero, Color.White);
                    float YOffset = 75;

                    for (int x = 0; x < state.SetupCharacters.Count; x++)
                    {
                        batch.Draw(_resources.GetTexture("LobbyCharBar").Texture, new Vector2(0, YOffset + 30 * x), Color.White);
                        batch.Draw(_resources.GetTexture("LobbyColorPreview").Texture, new Vector2(0, YOffset + 30 * x), state.SetupCharacters[x].PlayerColor);
                        if (state.SetupCharacters[x].PlayerType == PlayerType.Player)
                            _renderService.RenderText(DialogStrings.Player + (x + 1) + ": " + ProfileManager.AllProfiles[state.SetupCharacters[x].CharacterProfileIndex].Name, new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Black, Alignment.TopLeft, false);
                        else
                            _renderService.RenderText(DialogStrings.Player + (x + 1) + ": *" + ProfileManager.AllProfiles[state.SetupCharacters[x].CharacterProfileIndex].Name + "*", new Vector2(36, YOffset + 18 + 30 * x), _resources.GetFont("SegoeUIx14pt"), Color.Red, Alignment.TopLeft, false);
                    }
                    batch.Draw(_resources.GetTexture("LobbyOverlay").Texture, Vector2.Zero, Color.White);
                }

            });
        }
        private void unloadContent()
        {
            _resources.GetTexture("LobbyUnderlay").Unload();
            _resources.GetTexture("LobbyCharBar").Unload();
            _resources.GetTexture("LobbyColorPreview").Unload();
            _resources.GetTexture("LobbyOverlay").Unload();
        }
    }
}
