using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Slaam
{
    static class ScreenHelper
    {
        private static Screen CurrentScreen = new LogoScreen();
        private static Screen NextScreen;

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

        public static void ChangeScreen(Screen scrn)
        {
            ChangingScreens = true;
            NextScreen = scrn;
        }
    }
}
