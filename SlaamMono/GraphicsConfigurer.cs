using Microsoft.Xna.Framework;
using SlaamMono.Library;
using SlaamMono.x_;

namespace SlaamMono
{
    public class GraphicsConfigurer
    {
        private readonly StateReference<GraphicsDeviceManager> _graphicsState;

        public GraphicsConfigurer(StateReference<GraphicsDeviceManager> graphicsState)
        {
            _graphicsState = graphicsState;
        }

        public void ConfigureGraphics()
        {
            _graphicsState.State.IsFullScreen = false;
            _graphicsState.State.PreferredBackBufferWidth = GameGlobals.DRAWING_GAME_WIDTH;
            _graphicsState.State.PreferredBackBufferHeight = GameGlobals.DRAWING_GAME_HEIGHT;
            _graphicsState.State.PreferMultiSampling = false;
            _graphicsState.State.ApplyChanges();
            _graphicsState.MarkAsChanged();
        }
    }
}
