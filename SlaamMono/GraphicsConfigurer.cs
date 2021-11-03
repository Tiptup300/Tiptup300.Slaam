using SlaamMono.Library;
using SlaamMono.x_;

namespace SlaamMono
{
    public class GraphicsConfigurer : IGraphicsConfigurer
    {
        private readonly IGraphicsState _state;
        private readonly GraphicsConfiguration _config;

        public GraphicsConfigurer(IGraphicsState graphicsState, GraphicsConfiguration graphicsConfiguration)
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
