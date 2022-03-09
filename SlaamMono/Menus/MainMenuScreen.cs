using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using System;
using ZBlade;
using ZzziveGameEngine.StateManagement;
using ZzziveGameEngine.StateManagement.States;

namespace SlaamMono.Menus
{
    public class MainMenuScreen : IMainMenuScreen, IStatePerformer
    {
        private MainMenuScreenState _state;

        private readonly IScreenManager _screenManager;

        public MainMenuScreen(IScreenManager screenDirector)
        {
            _screenManager = screenDirector;
        }

        public void InitializeState()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Out;
            SlaamGame.mainBlade.TopMenu = buildMainMenu();
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;

            _state = new MainMenuScreenState();
        }

        private MenuItemTree buildMainMenu()
        {
            return new MenuItemTree(onInit: (mainMenu) =>
            {
                mainMenu.Nodes.Add(new MenuTextItem("Classic", onActivated: selectedClassicMode));
                mainMenu.Nodes.Add(new MenuTextItem("Survival", onActivated: selectedSurvival));
                mainMenu.Nodes.Add(new MenuItemTree(text: "Options", parent: mainMenu, onInit: (options) =>
                {
                    options.Nodes.Add(new MenuTextItem("Profiles", onActivated: selectedManageProfiles, isEnabled: false));
                    options.Nodes.Add(new MenuTextItem("View Highscores", onActivated: selectedHighscores));
                    options.Nodes.Add(new MenuTextItem("Credits", onActivated: selectedCredits));
                }));
                mainMenu.Nodes.Add(new MenuTextItem(text: "Exit", onActivated: exitGame));
            });
        }

        private void selectedCredits(object sender, EventArgs e) => _state.NextState = new CreditsScreenRequestState();
        private void selectedHighscores(object sender, EventArgs e) => _state.NextState = new HighScoreScreenRequestState();
        private void selectedManageProfiles(object sender, EventArgs e) => _state.NextState = new ProfileEditScreenRequestState();
        private void selectedSurvival(object sender, EventArgs e) => _state.NextState = new CharacterSelectionScreenState() { isForSurvival = true };
        private void selectedClassicMode(object sender, EventArgs e) => _state.NextState = new CharacterSelectionScreenRequestState();
        private void exitGame(object sender, EventArgs e) => _state.NextState = new GameExitState();


        public IState Perform()
        {
            if (_state.NextState != null)
            {
                return _state.NextState;
            }
            else
            {
                return _state;
            }
        }
        public void RenderState(SpriteBatch batch) { }

        public void Close()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            SlaamGame.mainBlade.TopMenu = null;
            SlaamGame.mainBlade.UserCanNavigateMenu = false;
        }
    }

}
