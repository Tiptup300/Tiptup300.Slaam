using Microsoft.Xna.Framework;
using SimpleInjector;
using SlaamMono.Library.Logging;
using SlaamMono.Screens;
using System;

namespace SlaamMono
{
    public class Bootstrap
    {
        public Container BuildContainer()
        {
            Container output;

            output = new Container();

            output.Register<IApp, SlaamGameApp>(Lifestyle.Singleton);
            output.Register<ISlaamGame, SlaamGame>(Lifestyle.Singleton);
            output.Register<ILoggingDevice, TextFileLoggingDevice>(Lifestyle.Singleton);
            output.Register<ILogger, Logger>(Lifestyle.Singleton);
            output.Register<MainMenuScreen>();

            output.Register<IScreenFactory, ScreenFactory>();

            return output;
        }
    }
}
