using Microsoft.Xna.Framework.Graphics;
using SlaamMono.CharacterSelection;
using SlaamMono.Helpers;
using System;
using ZBlade;

namespace SlaamMono.Screens
{
    public class MainMenuScreen : IScreen
    {
        private readonly IScreenFactory _screenFactory;
        private readonly IScreenManager _screenDirector;

        public MainMenuScreen(IScreenFactory screenFactory, IScreenManager screenDirector)
        {
            _screenFactory = screenFactory;
            _screenDirector = screenDirector;
        }

        public void Open()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Out;
            SlaamGame.mainBlade.TopMenu = buildMainMenu();
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;

            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
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

        private void selectedCredits(object sender, EventArgs e) => changeScreen(nameof(CreditsScreen));
        private void selectedHighscores(object sender, EventArgs e) => changeScreen(nameof(HighScoreScreen));
        private void selectedManageProfiles(object sender, EventArgs e) => changeScreen(nameof(ProfileEditScreen));
        private void selectedSurvival(object sender, EventArgs e) => changeScreen(nameof(SurvivalScreen));
        private void selectedClassicMode(object sender, EventArgs e) => changeScreen(nameof(ClassicCharSelectScreen));
        private void exitGame(object sender, EventArgs e) => SlaamGame.Instance.Exit();

        private void changeScreen(string screenName)
        {
            _screenDirector.ChangeTo(_screenFactory.Get(screenName));
        }

        public void Update() { }
        public void Draw(SpriteBatch batch) { }

        public void Close()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
            SlaamGame.mainBlade.TopMenu = null;
            SlaamGame.mainBlade.UserCanNavigateMenu = false;
        }
    }

}
