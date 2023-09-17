using System;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Logging;

namespace Tiptup300.Slaam;

public class SlamGameConfigurer
{
   private readonly IGraphicsConfigurer _graphicsConfigurer;
   private readonly SlaamGame _game;

   public SlamGameConfigurer(SlaamGame game, ILogger logger, IGraphicsConfigurer graphicsConfigurer)
   {
      _game = game;
      _graphicsConfigurer = graphicsConfigurer;
   }

   public void Run()
   {
      _graphicsConfigurer.ConfigureGraphics();
      _game.Run();
   }
}
