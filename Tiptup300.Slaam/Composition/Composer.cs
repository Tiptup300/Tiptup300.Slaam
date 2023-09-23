using Microsoft.Xna.Framework.Graphics;
using SimpleInjector;
using System.Tiptup300.Primitives;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Configurations;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Metrics;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.ResourceManagement;
using Tiptup300.Slaam.ResourceManagement.Loading;
using Tiptup300.Slaam.States.CharacterSelect.CharacterSelectBoxes;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.Composition;

public class Composer
{
   private Container? _container;

   public Container BuildContainer(ServiceLocator resolver)
   {
      _container = new Container();

      _container.RegisterInstance(resolver);
      register(800, 600);
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
      _container.RegisterInstance(new GameConfiguration());
      _container.Register<SlaamGameRunner>(Lifestyle.Singleton);
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
      _container.RegisterSingleton(typeof(IResolver<>), typeof(SlaamGameRunner).Assembly);
      _container.RegisterSingleton(typeof(IResolver<,>), typeof(SlaamGameRunner).Assembly);
   }

   private void registerResources()
   {
      _container.RegisterInstance(new Mut<ResourcesState>());
      _container.RegisterInstance(new ResourcesListsToLoad(new string[] { "botnames", "credits", "textures", "fonts", "boards" }));
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
      _container.Register<PlayerCharacterSelectBoxPerformer>(Lifestyle.Singleton);
   }
}
