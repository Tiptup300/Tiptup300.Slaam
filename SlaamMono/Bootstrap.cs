using SimpleInjector;
using SlaamMono.Library.Logging;
using SlaamMono.Resources;
using SlaamMono.Resources.Loading;
using SlaamMono.Screens;

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
            output.Register<ITextureLoader, TextureLoader>();
            output.Register<IPixelFactory, PixelFactory>();
            output.Register<ITextLineLoader, CommentedTextLineLoader>();
            output.Register<IFontLoader, FontLoader>();
            output.Register<ICachedTextureFactory, CachedTextureFactory>();
            output.Register<IResourceLoader, ResourceLoader>();
        }
    }
}
