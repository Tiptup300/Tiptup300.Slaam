using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class ProfileEditScreenPerformer : IStatePerformer
    {
        private ProfileEditScreenState _state = new ProfileEditScreenState();

        private readonly IResolver<MainMenuRequest, MainMenuScreenState> _menuStateResolver;
        private readonly IResources _resources;
        private readonly IRenderService _renderGraph;

        public ProfileEditScreenPerformer(
            IResolver<MainMenuRequest, MainMenuScreenState> menuStateResolver,
            IResources resources,
            IRenderService renderGraph)
        {
            _menuStateResolver = menuStateResolver;
            _resources = resources;
            _renderGraph = renderGraph;
        }

        public void InitializeState()
        {
            _state.MainMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), _resources, _renderGraph);
            _state.SubMenu = new Graph(new Rectangle(100, 200, GameGlobals.DRAWING_GAME_WIDTH - 100, 624), 2, new Color(0, 0, 0, 150), _resources, _renderGraph);
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

        public IState Perform()
        {
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
                    if (InputService.Instance.GetPlayers()[0].PressedUp)
                    {
                        _state.CurrentMenuChoice.Sub(1);
                        _state.MainMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    else if (InputService.Instance.GetPlayers()[0].PressedDown)
                    {
                        _state.CurrentMenuChoice.Add(1);
                        _state.MainMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    if (InputService.Instance.GetPlayers()[0].PressedAction)
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
                    if (InputService.Instance.GetPlayers()[0].PressedAction2)
                    {
                        return _menuStateResolver.Resolve(new MainMenuRequest());
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
                    if (InputService.Instance.GetPlayers()[0].PressedUp)
                    {
                        _state.CurrentMenuChoice.Sub(1);
                        _state.SubMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    else if (InputService.Instance.GetPlayers()[0].PressedDown)
                    {
                        _state.CurrentMenuChoice.Add(1);
                        _state.SubMenu.SetHighlight(_state.CurrentMenuChoice.Value);
                    }
                    if (InputService.Instance.GetPlayers()[0].PressedAction)
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
                    if (InputService.Instance.GetPlayers()[0].PressedAction2)
                    {
                        _state.CurrentMenu.Value = 0;
                        setupMainMenu();
                    }
                }
            }
            return _state;
        }

        public void RenderState(SpriteBatch batch)
        {

            if (_state.CurrentMenu.Value == 0)
                _state.MainMenu.Draw(batch);
            else
                _state.SubMenu.Draw(batch);
        }

        public void Close()
        {

        }


    }
}
