using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using System;
using ZBlade;

namespace SlaamMono.Screens
{
    public class MenuScreen : IScreen
    {

        public MenuScreen()
        {
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
            MenuItemTree output;

            output = new MenuItemTree("");
            MenuTextItem competition = new MenuTextItem("Competition");
            competition.Activated += selectedCompetitionMode;

            MenuTextItem survival = new MenuTextItem("Survival");
            survival.Activated += selectedSurvival;

            MenuItemTree options = new MenuItemTree("Options", output);
            {
                MenuTextItem manageProfiles = new MenuTextItem("Profiles");
                manageProfiles.Activated += selectedManageProfiles; 
                manageProfiles.IsEnabled = false;

                MenuTextItem highscores = new MenuTextItem("View Highscores");
                highscores.Activated += selectedHighscores;

                MenuTextItem credits = new MenuTextItem("Credits");
                credits.Activated += selectedCredits;

                options.Nodes.Add(manageProfiles);
                options.Nodes.Add(highscores);
                options.Nodes.Add(credits);
            }

            MenuTextItem exit = new MenuTextItem("Exit");
            exit.Activated += delegate { SlaamGame.Instance.Exit(); };

            output.Nodes.Add(competition);
            output.Nodes.Add(survival);
            output.Nodes.Add(options);
            output.Nodes.Add(exit);

            return output;
        }

        private void selectedCredits(object sender, EventArgs e) => changeScreen(new Credits(InstanceManager.Instance.Get<MenuScreen>()));
        private void selectedHighscores(object sender, EventArgs e) => changeScreen(new HighScoreScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>()));
        private void selectedManageProfiles(object sender, EventArgs e) => changeScreen(new ProfileEditScreen(InstanceManager.Instance.Get<MenuScreen>()));
        private void selectedSurvival(object sender, EventArgs e) => changeScreen(new SurvivalCharSelectScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>()));
        private void selectedCompetitionMode(object sender, EventArgs e) => changeScreen(new CharSelectScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>()));

        private void changeScreen(IScreen screen)
        {
            ScreenHelper.ChangeScreen(screen);
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
        }


        public void Update()
        {

        }

        public void Draw(SpriteBatch batch)
        {

        }

        public void Dispose()
        {
        }


    }

}
