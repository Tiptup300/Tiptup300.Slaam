using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Resources;

namespace SlaamMono.Resources
{
    public class WhitePixelResolver : IWhitePixelResolver
    {
        public WhitePixelResolver(ILogger logger)
        {
            _logger = logger;
        }

        private Texture2D _whitePixel;
        private readonly ILogger _logger;

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

            output = new Texture2D(SlaamGame.Graphics.GraphicsDevice, 1, 1);
            output.SetData(new Color[] { Color.White });

            return output;
        }

    }
}
