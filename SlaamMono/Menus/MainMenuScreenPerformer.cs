using Microsoft.Xna.Framework.Graphics;
using SlaamMono.MatchCreation;
using SlaamMono.Menus.Credits;
using SlaamMono.PlayerProfiles;
using System;
using ZBlade;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;
using ZzziveGameEngine.StateManagement.States;

namespace SlaamMono.Menus
{
    public class MainMenuScreenPerformer : IPerformer<MainMenuScreenState>
    {
        private MainMenuScreenState _state;
        private readonly IResolver<IRequest, IState> _stateResolver;

        public MainMenuScreenPerformer(IResolver<IRequest, IState> stateResolver)
        {
            _stateResolver = stateResolver;
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

        private void selectedCredits(object sender, EventArgs e) => _state.NextState = _stateResolver.Resolve(new CreditsRequest());
        private void selectedHighscores(object sender, EventArgs e) => _state.NextState = new HighScoreScreenRequestState();
        private void selectedManageProfiles(object sender, EventArgs e) => _state.NextState = new ProfileEditScreenRequestState();
        private void selectedSurvival(object sender, EventArgs e) => _state.NextState = new CharacterSelectionScreenState() { isForSurvival = true };
        private void selectedClassicMode(object sender, EventArgs e) => _state.NextState = new CharacterSelectionScreenRequestState();
        private void exitGame(object sender, EventArgs e) => _state.NextState = new GameExitState();


        public IState Perform(MainMenuScreenState state)
        {
            if (state.NextState != null)
            {
                return state.NextState;
            }
            else
            {
                return state;
            }
        }
        public void Render(MainMenuScreenState state) { }

        public void hideZBlade()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            SlaamGame.mainBlade.TopMenu = null;
            SlaamGame.mainBlade.UserCanNavigateMenu = false;
        }
    }

}
