using Microsoft.Xna.Framework;
using SlaamMono.Input;
using SlaamMono.Library;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Input;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Menus;
using SlaamMono.x_;
using System;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.PlayerProfiles
{
    public class ProfileEditScreenPerformer : IPerformer<ProfileEditScreenState>
    {
        private ProfileEditScreenState _state = new ProfileEditScreenState();

        private readonly IResolver<MainMenuRequest, IState> _menuStateResolver;
        private readonly IResources _resources;
        private readonly IRenderService _renderService;
        private readonly IInputService _inputService;

        public ProfileEditScreenPerformer(
            IResolver<MainMenuRequest, IState> menuStateResolver,
            IResources resources,
            IRenderService renderGraph,
            IInputService inputService)
        {
            _menuStateResolver = menuStateResolver;
            _resources = resources;
            _renderService = renderGraph;
            _inputService = inputService;
        }

        public void InitializeState()
        {
            _state.MainMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), _resources, _renderService);
            _state.SubMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), _resources, _renderService);
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
        private void setupMainMenu()
        {
            _state.MainMenu.Items.Columns.Clear();
            _state.MainMenu.Items.Columns.Add("PROFILES");
            _state.MainMenu.Items.Clear();
            for (int x = 1; x < ProfileManager.PlayableProfiles.Count; x++)
            {
                _state.MainMenu.Items.Add(true, new GraphItem(ProfileManager.PlayableProfiles[x].Name, x.ToString()));
            }
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

        public IState Perform(ProfileEditScreenState state)
        {
            if (state.CurrentMenu.Value == 0)
            {
                if (state.WaitingForQwerty)
                {
                    if (Qwerty.EditingString.Trim() != "")
                    {
                        ProfileManager.AddNewProfile(new PlayerProfile(Qwerty.EditingString, false));

                    }
                    state.WaitingForQwerty = false;
                    Qwerty.EditingString = "";
                    setupMainMenu();
                }
                else
                {
                    if (_inputService.GetPlayers()[0].PressedUp)
                    {
                        state.CurrentMenuChoice.Sub(1);
                        state.MainMenu.SetHighlight(state.CurrentMenuChoice.Value);
                    }
                    else if (_inputService.GetPlayers()[0].PressedDown)
                    {
                        state.CurrentMenuChoice.Add(1);
                        state.MainMenu.SetHighlight(state.CurrentMenuChoice.Value);
                    }
                    if (_inputService.GetPlayers()[0].PressedAction)
                    {
                        if (state.MainMenu.Items[state.CurrentMenuChoice.Value].Details[1] == "new")
                        {
                            Qwerty.DisplayBoard("");
                            state.WaitingForQwerty = true;
                        }
                        else
                        {
                            state.EditingProfile = int.Parse(state.MainMenu.Items[state.CurrentMenuChoice.Value].Details[1]);
                            state.CurrentMenu.Value = 1;
                            state.CurrentMenuChoice = new IntRange(0, 0, 2);
                            state.SubMenu.SetHighlight(0);
                        }
                    }
                    if (_inputService.GetPlayers()[0].PressedAction2)
                    {
                        return _menuStateResolver.Resolve(new MainMenuRequest());
                    }
                }
            }
            else if (state.CurrentMenu.Value == 1)
            {
                if (state.WaitingForQwerty)
                {
                    if (Qwerty.EditingString.Trim() != "")
                    {
                        ProfileManager.PlayableProfiles[state.EditingProfile].Name = Qwerty.EditingString;
                        ProfileManager.SaveProfiles();
                    }
                    state.WaitingForQwerty = false;
                    Qwerty.EditingString = "";

                    state.CurrentMenu.Value = 0;
                    setupMainMenu();
                }
                else
                {
                    if (_inputService.GetPlayers()[0].PressedUp)
                    {
                        state.CurrentMenuChoice.Sub(1);
                        state.SubMenu.SetHighlight(state.CurrentMenuChoice.Value);
                    }
                    else if (_inputService.GetPlayers()[0].PressedDown)
                    {
                        state.CurrentMenuChoice.Add(1);
                        state.SubMenu.SetHighlight(state.CurrentMenuChoice.Value);
                    }
                    if (_inputService.GetPlayers()[0].PressedAction)
                    {
                        if (state.SubMenu.Items[state.CurrentMenuChoice.Value].Details[1] == "del")
                        {
                            ProfileManager.RemovePlayer(ProfileManager.PlayableProfiles.GetRealIndex(state.EditingProfile));
                            state.EditingProfile = -1;
                            state.CurrentMenu.Value = 0;
                            setupMainMenu();
                        }
                        else if (state.SubMenu.Items[state.CurrentMenuChoice.Value].Details[1] == "ren")
                        {
                            Qwerty.DisplayBoard(ProfileManager.PlayableProfiles[state.EditingProfile].Name);
                            state.WaitingForQwerty = true;
                        }
                        else if (state.SubMenu.Items[state.CurrentMenuChoice.Value].Details[1] == "clr")
                        {
                            ProfileManager.PlayableProfiles[state.EditingProfile].TotalDeaths = 0;
                            ProfileManager.PlayableProfiles[state.EditingProfile].TotalGames = 0;
                            ProfileManager.PlayableProfiles[state.EditingProfile].TotalKills = 0;
                            ProfileManager.PlayableProfiles[state.EditingProfile].TotalPowerups = 0;
                            ProfileManager.PlayableProfiles[state.EditingProfile].BestGame = TimeSpan.Zero;
                            ProfileManager.SaveProfiles();
                            state.EditingProfile = -1;
                            state.CurrentMenu.Value = 0;
                            setupMainMenu();
                        }
                    }
                    if (_inputService.GetPlayers()[0].PressedAction2)
                    {
                        state.CurrentMenu.Value = 0;
                        setupMainMenu();
                    }
                }
            }
            return state;
        }

        public void RenderState()
        {
            _renderService.Render(batch =>
            {
                if (_state.CurrentMenu.Value == 0)
                {
                    _state.MainMenu.Draw(batch);
                }
                else
                {
                    _state.SubMenu.Draw(batch);
                }
            });
        }

        public void Close()
        {

        }


    }
}
