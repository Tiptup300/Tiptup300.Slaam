using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using System;

namespace SlaamMono.Screens
{
    public class ScreenDirector
    {
        public static ScreenDirector Instance = new ScreenDirector();

        private IScreen CurrentScreen = new LogoScreen(DI.Instance.Get<MainMenuScreen>());
        private IScreen NextScreen;

        private bool ChangingScreens = false;

        public void Update()
        {
            CurrentScreen.Update();

            if (ChangingScreens)
            {
                changeScreen();
            }
        }
        
        private void changeScreen()
        {
            ChangingScreens = false;
            CurrentScreen.Close();
            CurrentScreen = NextScreen;
            NextScreen = null;
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Normal);
            CurrentScreen.Open();
            GC.Collect();
        }

        public void Draw(SpriteBatch batch)
        {
            CurrentScreen.Draw(batch);
        }

        public void ChangeScreen(IScreen scrn)
        {
            ChangingScreens = true;
            NextScreen = scrn;
        }
    }
}
