#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using System;
using ZBlade;
#endregion

namespace SlaamMono
{
    public class MenuScreen : IScreen
    {
        private MenuItemTree _mainMenuList = new MenuItemTree("");

        public MenuScreen()
        {
        }

        public void Initialize()
        {
            if (_mainMenuList.Nodes.Count == 0)
            {
                MenuTextItem competition = new MenuTextItem("Competition");
                competition.Activated += delegate { ChangeScreen(new CharSelectScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>())); };

                MenuTextItem survival = new MenuTextItem("Survival");
                survival.Activated += delegate { ChangeScreen(new SurvivalCharSelectScreen(InstanceManager.Instance.Get<ILogger>(),InstanceManager.Instance.Get<MenuScreen>())); };

                MenuItemTree options = new MenuItemTree("Options", _mainMenuList);
                {
                    MenuTextItem manageProfiles = new MenuTextItem("Profiles");
                    manageProfiles.Activated += delegate { ChangeScreen(new ProfileEditScreen(InstanceManager.Instance.Get<MenuScreen>())); };
                    manageProfiles.IsEnabled = false;

                    MenuTextItem highscores = new MenuTextItem("View Highscores");
                    highscores.Activated += delegate { ChangeScreen(new HighScoreScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>())); };

                    MenuTextItem credits = new MenuTextItem("Credits");
                    credits.Activated += delegate { ChangeScreen(new Credits(InstanceManager.Instance.Get<MenuScreen>())); };

                    options.Nodes.Add(manageProfiles);
                    options.Nodes.Add(highscores);
                    options.Nodes.Add(credits);
                }

                _mainMenuList.Nodes.Add(competition);
                _mainMenuList.Nodes.Add(survival);
                _mainMenuList.Nodes.Add(options);

                MenuTextItem exit = new MenuTextItem("Exit");
                exit.Activated += delegate { SlaamGame.Instance.Exit(); };

                _mainMenuList.Nodes.Add(exit);
            }

            SlaamGame.mainBlade.Status = BladeStatus.Out;
            SlaamGame.mainBlade.TopMenu = _mainMenuList;
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;

            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
        }

        private void ChangeScreen(IScreen screen)
        {
            ScreenHelper.ChangeScreen(screen);
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
        }

        void competition_onSelected(object sender, EventArgs e)
        {
            ScreenHelper.ChangeScreen(new CharSelectScreen(InstanceManager.Instance.Get<ILogger>(), InstanceManager.Instance.Get<MenuScreen>()));
        }


        public void Update()
        {

        }


        public void Draw(SpriteBatch batch)
        {

        }


        private void DrawBoard(SpriteBatch batch, Vector2 position, string text)
        {
            Vector2 drawOrigin = new Vector2(Resources.MenuChoice.Width / 2f - 0.5f, Resources.MenuChoice.Height / 2f);
            Vector2 drawOrigin2 = new Vector2(Resources.MenuChoiceGlow.Width / 2f - 0.5f, Resources.MenuChoiceGlow.Height / 2f);

            batch.Draw(Resources.MenuChoiceGlow.Texture, position + new Vector2(0, Resources.MenuChoice.Height), null, Color.White, 0f, drawOrigin2, 1f, SpriteEffects.FlipVertically, 0);
            batch.Draw(Resources.MenuChoiceGlow.Texture, position, null, Color.White, 0f, drawOrigin2, 1f, SpriteEffects.None, 0);

            batch.Draw(Resources.MenuChoice.Texture, position, null, Color.White, 0f, drawOrigin, 1f, SpriteEffects.None, 0);
            batch.Draw(Resources.MenuChoice.Texture, position + new Vector2(0, Resources.MenuChoice.Height), null, Color.White, 0f, drawOrigin, 1f, SpriteEffects.FlipVertically, 0);

            batch.DrawString(Resources.SegoeUIx48ptBold, text, position, Color.White, 0f, Resources.SegoeUIx48ptBold.MeasureString(text) / 2f, 1f, SpriteEffects.None, 0);
            batch.DrawString(Resources.SegoeUIx48ptBold, text, position + new Vector2(0, Resources.MenuChoice.Height), Color.White, 0f, Resources.SegoeUIx48ptBold.MeasureString(text) / 2f, 1f, SpriteEffects.FlipVertically, 0);
        }


        public void Dispose()
        {
        }


    }

}
