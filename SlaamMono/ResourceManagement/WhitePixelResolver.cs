using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.ResourceManagement
{
    public class WhitePixelResolver : IResolver<WhitePixelRequest, Texture2D>
    {
        private Texture2D _whitePixel;

        private readonly ILogger _logger;
        private readonly IGraphicsState _graphics;

        public WhitePixelResolver(ILogger logger, IGraphicsState graphics)
        {
            _logger = logger;
            _graphics = graphics;
        }

        public Texture2D Resolve(WhitePixelRequest request)
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

            output = new Texture2D(_graphics.Get().GraphicsDevice, 1, 1);
            output.SetData(new Color[] { Color.White });

            return output;
        }
    }
}
