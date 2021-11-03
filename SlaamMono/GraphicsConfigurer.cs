using SlaamMono.Library;
using SlaamMono.Library.Configurations;
using SlaamMono.x_;

namespace SlaamMono
{
    public class GraphicsConfigurer : IGraphicsConfigurer
    {
        private readonly IGraphicsState _state;
        private readonly GraphicsConfig _config;

        public GraphicsConfigurer(IGraphicsState graphicsState, GraphicsConfig graphicsConfiguration)
        {
            _state = graphicsState;
            _config = graphicsConfiguration;
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
