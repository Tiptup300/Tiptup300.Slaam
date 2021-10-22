using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using SlaamMono.Screens;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Testing
{
    public class ScreenDirectorTests
    {
        [Test]
        public void CanUpdateScreen()
        {
            TestScreen screen = new TestScreen();
            ScreenManager screenDirector = new ScreenManager();

            screenDirector.ChangeTo(screen);
            screenDirector.Update();

            Assert.True(screen.__Update__Ran);
        }

        [Test]
        public void CanDrawScreen()
        {
            TestScreen screen = new TestScreen();
            ScreenManager screenDirector = new ScreenManager();

            screenDirector.ChangeTo(screen);
            screenDirector.Update();
            screenDirector.Draw(null);

            Assert.True(screen.__Draw__Ran);
        }

        [Test]
        public void CanChangeScreens()
        {
            TestScreen firstScreen = new TestScreen();
            TestScreen secondScreen = new TestScreen();
            ScreenManager screenDirector = new ScreenManager();

            screenDirector.ChangeTo(firstScreen);
            screenDirector.Update();
            screenDirector.ChangeTo(secondScreen);
            screenDirector.Update();

            Assert.True(firstScreen.__Update__Ran);
            Assert.True(firstScreen.__Close__Ran);
            Assert.True(secondScreen.__Update__Ran);
            Assert.False(secondScreen.__Close__Ran);
        }
    }
}
