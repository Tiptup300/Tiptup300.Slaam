using Microsoft.Xna.Framework.Graphics;
using SimpleInjector;
using SlaamMono.Library.Logging;
using SlaamMono.Resources;
using SlaamMono.Resources.Loading;
using SlaamMono.Screens;
using System.Collections.Generic;

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

            registerScreens(output);
            registerResources(output);

            return output;
        }

        private void registerScreens(Container output)
        {
            output.Register<MainMenuScreen>();
            output.Register<CreditsScreen>();
            output.Register<HighScoreScreen>();
            output.Register<ProfileEditScreen>();
            output.Register<SurvivalCharSelectScreen>();
            output.Register<ClassicCharSelectScreen>();
            output.Register<IScreenDirector, ScreenDirector>(Lifestyle.Singleton);
            output.Register<IFirstScreenResolver, FirstScreenResolver>(Lifestyle.Singleton);
            output.Register<LogoScreen>();
            output.Register<IScreenFactory, ScreenFactory>(Lifestyle.Singleton);
        }

        private void registerResources(Container output)
        {
            output.Register<IFileLoader<Texture2D>, Texture2DLoader>();
            output.Register<IPixelFactory, PixelFactory>();
            output.Register<IFileLoader<IEnumerable<string>>, CommentedTextLineLoader>();
            output.Register<IFileLoader<SpriteFont>, FontLoader>();
            output.Register<IFileLoader<CachedTexture>, CachedTextureLoader>();
            output.Register<IResourceLoader, ResourceLoader>();
        }
    }
}
