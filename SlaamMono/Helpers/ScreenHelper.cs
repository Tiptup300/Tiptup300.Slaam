using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono
{
    static class ScreenHelper
    {
        private static IScreen CurrentScreen = new LogoScreen();
        private static IScreen NextScreen;

        private static bool ChangingScreens = false;

        public static void Update()
        {
            CurrentScreen.Update();

            if (ChangingScreens)
            {
                ChangingScreens = false;
                CurrentScreen.Dispose();
                CurrentScreen = NextScreen;
                NextScreen = null;
                BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Normal);
                CurrentScreen.Initialize();
                GC.Collect();
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            CurrentScreen.Draw(batch);

        }

        public static void ChangeScreen(IScreen scrn)
        {
            ChangingScreens = true;
            NextScreen = scrn;
        }
    }
}
