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
        public MenuItemTree MainMenuList = new MenuItemTree("");
        public MenuItemTree CurrentTree;

        public static MenuScreen Instance { get { return _instance; } }
        private static MenuScreen _instance;

        public static void InitInstance(ILogger logger)
        {
            _instance = new MenuScreen(logger);
        }

        private readonly ILogger _logger;

        public MenuScreen(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            if (MainMenuList.Nodes.Count == 0)
            {
                MenuTextItem competition = new MenuTextItem("Competition");
                competition.Activated += delegate { ChangeScreen(new CharSelectScreen(InstanceManager.Instance.Get<ILogger>())); };

                MenuTextItem survival = new MenuTextItem("Survival");
                survival.Activated += delegate { ChangeScreen(new SurvivalCharSelectScreen(InstanceManager.Instance.Get<ILogger>())); };


                MenuItemTree options = new MenuItemTree("Options", MainMenuList);
                {
                    MenuTextItem manageProfiles = new MenuTextItem("Profiles");
                    manageProfiles.Activated += delegate { ChangeScreen(new ProfileEditScreen()); };
                    manageProfiles.IsEnabled = false;

                    MenuTextItem highscores = new MenuTextItem("View Highscores");
                    highscores.Activated += delegate { ChangeScreen(new HighScoreScreen(InstanceManager.Instance.Get<ILogger>())); };

                    MenuTextItem credits = new MenuTextItem("Credits");
                    credits.Activated += delegate { ChangeScreen(new Credits()); };

#if ZUNE
                    //MenuSongLibraryItem library = new MenuSongLibraryItem("Custom Soundtrack", options);
                    //options.Nodes.Add(library);
#endif
                    options.Nodes.Add(manageProfiles);
                    options.Nodes.Add(highscores);
                    options.Nodes.Add(credits);
                }

                MainMenuList.Nodes.Add(competition);
                MainMenuList.Nodes.Add(survival);
                MainMenuList.Nodes.Add(options);



                MenuTextItem exit = new MenuTextItem("Exit");
                exit.Activated += delegate { SlaamGame.Instance.Exit(); };



                MainMenuList.Nodes.Add(exit);

            }

#if ZUNE
            SlaamGame.mainBlade.Status = BladeStatus.Out;
            SlaamGame.mainBlade.TopMenu = MainMenuList;
            SlaamGame.mainBlade.UserCanNavigateMenu = true;
            SlaamGame.mainBlade.UserCanCloseMenu = false;
#else
            CurrentTree = MainMenuList;
            FeedManager.InitializeFeeds(DialogStrings.MenuScreenFeed);
#endif

            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Menu);
        }

        private void ChangeScreen(IScreen screen)
        {
            ScreenHelper.ChangeScreen(screen);

#if ZUNE
            SlaamGame.mainBlade.Status = BladeStatus.Hidden;
#endif
        }

        void competition_onSelected(object sender, EventArgs e)
        {
            ScreenHelper.ChangeScreen(new CharSelectScreen(InstanceManager.Instance.Get<ILogger>()));
        }


        public void Update()
        {
#if !ZUNE
            MenuChoiceOffset.Update(FPSManager.MovementFactorTimeSpan);
            if (MenuChoiceOffset.Goal != Vector2.Zero && MenuChoiceOffset.IsFinished())
            {
                if (MenuChoiceOffset.Goal.X < 0)
                    SelectedIndex++;
                else if(MenuChoiceOffset.Goal.X > 0)
                    SelectedIndex--;

                MenuChoiceOffset.Goal = Vector2.Zero;
                MenuChoiceOffset.Update(FPSManager.MovementFactorTimeSpan);
            }

            if (InputComponent.Players[0].PressingLeft && MenuChoiceOffset.IsFinished())
            {
                MenuChoiceOffset.Goal = new Vector2(Resources.MenuChoiceGlow.Width,0);
                MenuChoiceOffset.Reset();
            }
            else if (InputComponent.Players[0].PressingRight && MenuChoiceOffset.IsFinished())
            {
                MenuChoiceOffset.Goal = new Vector2(-Resources.MenuChoiceGlow.Width,0);
                MenuChoiceOffset.Reset();
            }
            else if (InputComponent.Players[0].PressedAction && MenuChoiceOffset.IsFinished())
            {
                if (CurrentTree.Nodes[SelectedIndex].GetType() == typeof(MenuItemTree))
                {
                    CurrentTree = (MenuItemTree)CurrentTree.Nodes[SelectedIndex];
                    SelectedIndex = 0;
                }
                else
                {
                    MenuItem item = (MenuItem)CurrentTree.Nodes[SelectedIndex];
                    item.DetectInput(ZuneButtons.PadCenter);
                }
            }
            else if (InputComponent.Players[0].PressedAction2 && CurrentTree.Parent != null && MenuChoiceOffset.IsFinished())
            {
                CurrentTree = CurrentTree.Parent;
                SelectedIndex = 0;
            }
#endif
        }


        public void Draw(SpriteBatch batch)
        {
#if !ZUNE
            Vector2 Center = new Vector2(GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT) / 2f;
            DrawBoard(batch, MenuChoiceOffset.Position + new Vector2(Center.X - Resources.MenuChoiceGlow.Width * 2, Center.Y), CurrentTree.Nodes[SelectedIndex - 2].ToString());
            DrawBoard(batch, MenuChoiceOffset.Position + new Vector2(Center.X - Resources.MenuChoiceGlow.Width, Center.Y), CurrentTree.Nodes[SelectedIndex - 1].ToString());
            DrawBoard(batch, MenuChoiceOffset.Position + Center, CurrentTree.Nodes[SelectedIndex].ToString());
            DrawBoard(batch, MenuChoiceOffset.Position + new Vector2(Center.X + Resources.MenuChoiceGlow.Width, Center.Y), CurrentTree.Nodes[SelectedIndex + 1].ToString());
            DrawBoard(batch, MenuChoiceOffset.Position + new Vector2(Center.X + Resources.MenuChoiceGlow.Width * 2, Center.Y), CurrentTree.Nodes[SelectedIndex + 2].ToString());
            
            
            for (int x = 0; x < 5; x++)
            {
                //batch.DrawString(Resources.SegoeUIx48ptBold, CurrentTree.Nodes[x+SelectedIndex-2].ToString(), new Vector2(100, 400 + x* 50), (x == 2 ? Color.Black : Color.Blue));
            }

            batch.Draw(Resources.MenuOverlay.Texture, Vector2.Zero, Color.White);
#endif
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
