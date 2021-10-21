using Microsoft.Xna.Framework.Graphics;
using SimpleInjector;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Logging;
using SlaamMono.Resources;
using SlaamMono.Resources.Loading;
using SlaamMono.Screens;
using System.Collections.Generic;

namespace SlaamMono
{
    public class Bootstrap
    {
        private Container _container;

        public Container BuildContainer()
        {
            _container = new Container();

            register();
            registerComponents();
            registerScreens();
            registerResources();
            registerGameplay();

            return _container;
        }

        private void register()
        {
            _container.Register<IApp, SlaamGameApp>(Lifestyle.Singleton);
            _container.Register<ISlaamGame, SlaamGame>(Lifestyle.Singleton);
            _container.Register<ILoggingDevice, TextFileLoggingDevice>(Lifestyle.Singleton);
            _container.Register<ILogger, Logger>(Lifestyle.Singleton);
        }

        public void registerComponents()
        {
            _container.Register<ITextRenderer, TextManager>(Lifestyle.Singleton);
        }

        private void registerScreens()
        {
            _container.Register<MainMenuScreen>();
            _container.Register<CreditsScreen>();
            _container.Register<HighScoreScreen>();
            _container.Register<ProfileEditScreen>();
            _container.Register<SurvivalCharSelectScreen>();
            _container.Register<ClassicCharSelectScreen>();
            _container.Register<IScreenDirector, ScreenDirector>(Lifestyle.Singleton);
            _container.Register<IFirstScreenResolver, FirstScreenResolver>(Lifestyle.Singleton);
            _container.Register<LogoScreen>();
            _container.Register<IScreenFactory, ScreenFactory>(Lifestyle.Singleton);
        }

        private void registerResources()
        {
            _container.Register<ResourceManager>(Lifestyle.Singleton);
            _container.Register<IFileLoader<Texture2D>, Texture2DLoader>(Lifestyle.Singleton);
            _container.Register<IPixelFactory, PixelFactory>(Lifestyle.Singleton);
            _container.Register<IFileLoader<IEnumerable<string>>, CommentedTextLineLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<SpriteFont>, FontLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<CachedTexture>, CachedTextureLoader>(Lifestyle.Singleton);
            _container.Register<IResourceLoader, ResourceLoader>(Lifestyle.Singleton);
        }

        private void registerGameplay()
        {
            _container.Register<PlayerColorResolver>(Lifestyle.Singleton);
        }
    }
}
