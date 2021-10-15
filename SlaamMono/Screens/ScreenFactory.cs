using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Screens
{
    public class ScreenFactory : IScreenFactory
    {
        public ScreenFactory()
        {

        }

        public IScreen Get(string name)
        {
            switch(name)
            {
                case "credits": return new Credits(DI.Instance.Get<MenuScreen>());
                case "profiles": return new ProfileEditScreen(DI.Instance.Get<MenuScreen>());
                case "highscores": return new HighScoreScreen(DI.Instance.Get<ILogger>(), DI.Instance.Get<MenuScreen>());
                case "survival-mode": return new SurvivalCharSelectScreen(DI.Instance.Get<ILogger>(), DI.Instance.Get<MenuScreen>());
                case "classic-mode": return new CharSelectScreen(DI.Instance.Get<ILogger>(), DI.Instance.Get<MenuScreen>());
                default:
                    throw new Exception("Screen Name Not Expected!");
            }
        }
    }
}
