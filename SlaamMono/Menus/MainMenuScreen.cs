using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using SlaamMono.PlayerProfiles;
using SlaamMono.Survival;
using SlaamMono.x_;
using System;
using ZBlade;

namespace SlaamMono.Menus
{
    public class MainMenuScreen : IMainMenuScreen, IStateUpdater
    {
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

        private void selectedCredits(object sender, EventArgs e) => _screenManager.ChangeTo<CreditsScreen>();
        private void selectedHighscores(object sender, EventArgs e) => _screenManager.ChangeTo<HighScoreScreen>();
        private void selectedManageProfiles(object sender, EventArgs e) => _screenManager.ChangeTo<ProfileEditScreen>();
        private void selectedSurvival(object sender, EventArgs e) => _screenManager.ChangeTo<SurvivalCharacterSelectionScreen>();
        private void selectedClassicMode(object sender, EventArgs e) => _screenManager.ChangeTo<CharacterSelectionScreen>();
        private void exitGame(object sender, EventArgs e) => SlaamGame.Instance.Exit();


        public void UpdateState() { }
        public void RenderState(SpriteBatch batch) { }

        public void Close()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            SlaamGame.mainBlade.TopMenu = null;
            SlaamGame.mainBlade.UserCanNavigateMenu = false;
        }
    }

}
