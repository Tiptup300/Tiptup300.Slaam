using Microsoft.Xna.Framework.Graphics;
using SimpleInjector;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Configurations;
using SlaamMono.Library.Graphics;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Metrics;
using SlaamMono.PlayerProfiles;
using SlaamMono.ResourceManagement;
using SlaamMono.ResourceManagement.Loading;
using SlaamMono.x_;
using ZzziveGameEngine;

namespace SlaamMono.Composition
{
    public class Composer
    {
        private Container _container;

        public Container BuildContainer(ServiceLocator resolver)
        {
            _container = new Container();

            _container.RegisterInstance(resolver);
            register();
            registerComponents();
            registerScreens();
            registerResources();
            registerGameplay();

            _container.Verify();

            return _container;
        }

        private void register(int width, int height)
        {
            _container.RegisterInstance(new ProfileFileVersion(new byte[] { 000, 000, 000, 002, }));
            _container.RegisterInstance(new GameConfig(true));
            _container.Register<SlamGameConfigurer>(Lifestyle.Singleton);
            _container.Register<SlaamGame>(Lifestyle.Singleton);
            _container.Register<IGraphicsStateService, GraphicsStateService>(Lifestyle.Singleton);
            _container.RegisterInstance(new GraphicsConfig(width, height, false, false));
            _container.Register<IGraphicsConfigurer, GraphicsConfigurer>(Lifestyle.Singleton);
            _container.Register<ILoggingDevice, TextFileLoggingDevice>(Lifestyle.Singleton);
            _container.Register<ILogger, Logger>(Lifestyle.Singleton);
        }

        public void registerComponents()
        {
            _container.Register<RenderService>(Lifestyle.Singleton);
            _container.Register<IRenderService>(() => _container.GetInstance<RenderService>(), Lifestyle.Singleton);

            _container.Register<InputService>(Lifestyle.Singleton);
            _container.Register<IInputService>(() => _container.GetInstance<InputService>(), Lifestyle.Singleton);

            _container.Register<FrameTimeService>(Lifestyle.Singleton);
            _container.Register<IFrameTimeService>(() => _container.GetInstance<FrameTimeService>(), Lifestyle.Singleton);

            _container.Register<FpsRenderer>(Lifestyle.Singleton);
        }

        private void registerScreens()
        {
            // Register all IRequests
            _container.RegisterSingleton(typeof(IResolver<>), typeof(SlamGameConfigurer).Assembly);
            _container.RegisterSingleton(typeof(IResolver<,>), typeof(SlamGameConfigurer).Assembly);
        }

        private void registerResources()
        {
            _container.RegisterInstance(new Mut<ResourcesState>());
            _container.RegisterInstance(new ResourcesListsToLoad(new string[] { "BotNames", "Credits", "Textures", "Fonts", "BoardList" }));
            _container.Register<IResources, Resources>(Lifestyle.Singleton);
            _container.Register<IFileLoader<Texture2D>, Texture2DLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<string[]>, CommentedTextLineLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<SpriteFont>, FontLoader>(Lifestyle.Singleton);
            _container.Register<IFileLoader<CachedTexture>, CachedTextureLoader>(Lifestyle.Singleton);
            _container.Register<IResourceLoader, ResourceLoader>(Lifestyle.Singleton);
            _container.Register<IResolver<TextureRequest, CachedTexture>, CachedTextureRequestHandler>(Lifestyle.Singleton);
        }

        private void registerGameplay()
        {
            _container.Register<PlayerColorResolver>(Lifestyle.Singleton);
        }
    }
}
