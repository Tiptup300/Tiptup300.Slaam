using Microsoft.Xna.Framework;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Logging;

namespace Tiptup300.Slaam;

public class SlamGameConfigurer
{
   private readonly IGraphicsConfigurer _graphicsConfigurer;
   private readonly SlaamGame _game;
   private readonly IGraphicsStateService _graphicsStateService;

   public SlamGameConfigurer(SlaamGame game, ILogger logger, IGraphicsConfigurer graphicsConfigurer, IGraphicsStateService graphicsStateService)
   {
      _game = game;
      _graphicsConfigurer = graphicsConfigurer;
      _graphicsStateService = graphicsStateService;
   }

   public void Run()
   {
      _graphicsStateService.Set(new GraphicsDeviceManager(_game));
      _graphicsConfigurer.ConfigureGraphics();


      _game.Run();
   }
}
