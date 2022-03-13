using SlaamMono.Library;
using SlaamMono.Library.Configurations;

namespace SlaamMono
{
    public class GraphicsConfigurer : IGraphicsConfigurer
    {
        private readonly IGraphicsState _state;
        private readonly GraphicsConfig _config;

        public GraphicsConfigurer(IGraphicsState state, GraphicsConfig config)
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
}
