using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.Primitives;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.ResourceManagement;

public class WhitePixelResolver : IResolver<WhitePixelRequest, Texture2D>
{
   private Texture2D _whitePixel;

   private readonly ILogger _logger;
   private readonly IGraphicsStateService _graphics;

   public WhitePixelResolver(ILogger logger, IGraphicsStateService graphics)
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
