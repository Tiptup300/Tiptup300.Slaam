using Microsoft.Xna.Framework.Graphics;
using System;
using ZBlade;

namespace SlaamMono.Screens
{
    public class MainMenuScreen : IScreen
    {
        private readonly IScreenFactory _screenFactory;

        public MainMenuScreen(IScreenFactory screenFactory)
        {
            _screenFactory = screenFactory;
        }

        public void Initialize()
        {
            SlaamGame.mainBlade.Status = BladeStatus.Out;
            SlaamGame.mainBlade.TopMenu = buildMainMenuNodes();
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;

            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
        }

        private MenuItemTree buildMainMenuNodes()
        {
            return new MenuItemTree(onInit: (mainMenu) =>
            {
                mainMenu.Nodes.Add(new MenuTextItem("Competition", onActivated: selectedCompetitionMode));
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

        private void selectedCredits(object sender, EventArgs e) => changeScreen("credits");
        private void selectedHighscores(object sender, EventArgs e) => changeScreen("highscores");
        private void selectedManageProfiles(object sender, EventArgs e) => changeScreen("profiles");
        private void selectedSurvival(object sender, EventArgs e) => changeScreen("survival-mode");
        private void selectedCompetitionMode(object sender, EventArgs e) => changeScreen("classic-mode");
        private void exitGame(object sender, EventArgs e) => SlaamGame.Instance.Exit();

        private void changeScreen(string screenName)
        {
            ScreenHelper.ChangeScreen(_screenFactory.Get(screenName));
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
        }
        public void Update() { }
        public void Draw(SpriteBatch batch) { }
        public void Dispose() { }


    }

}
