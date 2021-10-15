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

            output.Register<IApp, SlaamGameApp>();
            output.Register<ISlaamGame, SlaamGame>();
            output.Register<MainMenuScreen>();
            output.Register<ILoggingDevice, TextFileLoggingDevice>();
            output.Register<ILogger, Logger>();

            output.Register<IScreenFactory, ScreenFactory>();

            return output;
        }
    }
}
