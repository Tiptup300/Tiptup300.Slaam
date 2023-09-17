using SlaamMono.Library.Configurations;
using SlaamMono.Library.Graphics;

namespace SlaamMono;

public class GraphicsConfigurer : IGraphicsConfigurer
{
   private readonly IGraphicsStateService _state;
   private readonly GraphicsConfig _config;

   public GraphicsConfigurer(IGraphicsStateService state, GraphicsConfig config)
   {
      _state = state;
      _config = config;
   }

   public void ConfigureGraphics()
   {
      _state.ApplyChanges(graphics =>
      {
         graphics.IsFullScreen = _config.IsFullScreen;
         graphics.PreferredBackBufferWidth = _config.RenderWidth;
         graphics.PreferredBackBufferHeight = _config.RenderHeight;
         graphics.PreferMultiSampling = _config.MultiSampling;
      });
   }
}
