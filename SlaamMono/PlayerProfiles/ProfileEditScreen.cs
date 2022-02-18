using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Input;
using SlaamMono.Library;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.x_;
using System;

namespace SlaamMono.PlayerProfiles
{
    class ProfileEditScreen : ILogic
    {
        public static ProfileEditScreen Instance =
            new ProfileEditScreen(
                x_Di.Get<IScreenManager>(),
                x_Di.Get<IResources>(),
                x_Di.Get<IRenderGraph>());

        public bool SetupNewProfile { get => _state.SetupNewProfile; set { _state.SetupNewProfile = value; } }

        private ProfileEditScreenState _state = new ProfileEditScreenState();


        private readonly IScreenManager _screenDirector;

        public ProfileEditScreen(IScreenManager screenDirector, IResources resourcesManager, IRenderGraph renderGraphManager)
        {
            _screenDirector = screenDirector;
            _state.MainMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), resourcesManager, renderGraphManager);
            _state.SubMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), resourcesManager, renderGraphManager);
        }

        public void InitializeState()
        {
            BackgroundManager.ChangeBG(BackgroundType.Menu);

            setupMainMenu();
            resetSubMenu();
            if (_state.SetupNewProfile)
            {
                _state.SetupNewProfile = false;
                _state.CurrentMenu.Value = 0;
                _state.WaitingForQwerty = true;
                Qwerty.DisplayBoard("");
            }
        }

        public void UpdateState()
        {
            BackgroundManager.SetRotation(1f);

            if (_state.CurrentMenu.Value == 0)
            {
                if (_state.WaitingForQwerty)
                {
                    if (Qwerty.EditingString.Trim() != "")
                    {
                        ProfileManager.AddNewProfile(new PlayerProfile(Qwerty.EditingString, false));

                    }
                    _state.WaitingForQwerty = false;
                    Qwerty.EditingString = "";
                    setupMainMenu();
                }
                else
                {
                    if (InputComponent.Players[0].PressedUp)
                    {
                        _state.CurrentMenuChoice.Sub(1);
                        _state.MainMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    else if (InputComponent.Players[0].PressedDown)
                    {
                        _state.CurrentMenuChoice.Add(1);
                        _state.MainMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    if (InputComponent.Players[0].PressedAction)
                    {
                        if (_state.MainMenu.Items[_state.CurrentMenuChoice.Value].Details[1] == "new")
                        {
                            Qwerty.DisplayBoard("");
                            _state.WaitingForQwerty = true;
                        }
                        else
                        {
                            _state.EditingProfile = int.Parse(_state.MainMenu.Items[_state.CurrentMenuChoice.Value].Details[1]);
                            _state.CurrentMenu.Value = 1;
                            _state.CurrentMenuChoice = new IntRange(0, 0, 2);
                            _state.SubMenu.SetHighlight(0);
                        }
                    }
                    if (InputComponent.Players[0].PressedAction2)
                    {
                        _screenDirector.ChangeTo<IMainMenuScreen>();
                    }
                }
            }
            else if (_state.CurrentMenu.Value == 1)
            {
                if (_state.WaitingForQwerty)
                {
                    if (Qwerty.EditingString.Trim() != "")
                    {
                        ProfileManager.PlayableProfiles[_state.EditingProfile].Name = Qwerty.EditingString;
                        ProfileManager.SaveProfiles();
                    }
                    _state.WaitingForQwerty = false;
                    Qwerty.EditingString = "";

                    _state.CurrentMenu.Value = 0;
                    setupMainMenu();
                }
                else
                {
                    if (InputComponent.Players[0].PressedUp)
                    {
                        _state.CurrentMenuChoice.Sub(1);
                        _state.SubMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    else if (InputComponent.Players[0].PressedDown)
                    {
                        _state.CurrentMenuChoice.Add(1);
                        _state.SubMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    if (InputComponent.Players[0].PressedAction)
                    {
                        if (_state.SubMenu.Items[_state.CurrentMenuChoice.Value].Details[1] == "del")
                        {
                            ProfileManager.RemovePlayer(ProfileManager.PlayableProfiles.GetRealIndex(_state.EditingProfile));
                            _state.EditingProfile = -1;
                            _state.CurrentMenu.Value = 0;
                            setupMainMenu();
                        }
                        else if (_state.SubMenu.Items[_state.CurrentMenuChoice.Value].Details[1] == "ren")
                        {
                            Qwerty.DisplayBoard(ProfileManager.PlayableProfiles[_state.EditingProfile].Name);
                            _state.WaitingForQwerty = true;
                        }
                        else if (_state.SubMenu.Items[_state.CurrentMenuChoice.Value].Details[1] == "clr")
                        {
                            ProfileManager.PlayableProfiles[_state.EditingProfile].TotalDeaths = 0;
                            ProfileManager.PlayableProfiles[_state.EditingProfile].TotalGames = 0;
                            ProfileManager.PlayableProfiles[_state.EditingProfile].TotalKills = 0;
                            ProfileManager.PlayableProfiles[_state.EditingProfile].TotalPowerups = 0;
                            ProfileManager.PlayableProfiles[_state.EditingProfile].BestGame = TimeSpan.Zero;
                            ProfileManager.SaveProfiles();
                            _state.EditingProfile = -1;
                            _state.CurrentMenu.Value = 0;
                            setupMainMenu();
                        }
                    }
                    if (InputComponent.Players[0].PressedAction2)
                    {
                        _state.CurrentMenu.Value = 0;
                        setupMainMenu();
                    }
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {

            if (_state.CurrentMenu.Value == 0)
                _state.MainMenu.Draw(batch);
            else
                _state.SubMenu.Draw(batch);
        }

        public void Close()
        {

        }

        private void setupMainMenu()
        {
            _state.MainMenu.Items.Columns.Clear();
            _state.MainMenu.Items.Columns.Add("PROFILES");
            _state.MainMenu.Items.Clear();
            for (int x = 1; x < ProfileManager.PlayableProfiles.Count; x++)
                _state.MainMenu.Items.Add(true, new GraphItem(ProfileManager.PlayableProfiles[x].Name, x.ToString()));
            _state.MainMenu.Items.Add(true, new GraphItem("Create New Profile...", "new"));
            _state.MainMenu.SetHighlight(0);
            _state.CurrentMenuChoice = new IntRange(0, 0, _state.MainMenu.Items.Count - 1);
        }

        private void resetSubMenu()
        {
            _state.SubMenu.Items.Clear();
            _state.SubMenu.Items.Columns.Clear();
            _state.SubMenu.Items.Columns.Add("OPTIONS");
            _state.SubMenu.Items.Add(true, new GraphItem("Rename", "ren"), new GraphItem("Delete", "del"), new GraphItem("Clear Stats", "clr"));
            _state.SubMenu.CalculateBlocks();
        }
    }
}
