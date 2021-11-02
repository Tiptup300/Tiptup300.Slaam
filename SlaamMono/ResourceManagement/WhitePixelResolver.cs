using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.ResourceManagement
{
    public class WhitePixelResolver : IWhitePixelResolver
    {
        public WhitePixelResolver(ILogger logger, StateReference<GraphicsDeviceManager> graphics)
        {
            _logger = logger;
            _graphics = graphics;
        }

        private Texture2D _whitePixel;
        private readonly ILogger _logger;
        private readonly StateReference<GraphicsDeviceManager> _graphics;

        public Texture2D GetWhitePixel()
        {
            if (_whitePixel is null)
            {
                _whitePixel = buildWhitePixel();
                _logger.Log(" - Dot Image Created.");
            }
            return _whitePixel;
        }

        private Texture2D buildWhitePixel()
        {
            Texture2D output;

            output = new Texture2D(_graphics.State.GraphicsDevice, 1, 1);
            output.SetData(new Color[] { Color.White });

            return output;
        }

    }
}
