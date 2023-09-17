using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.ResourceManagement.Loading;

public class CachedTextureLoader : IFileLoader<CachedTexture>
{
   private readonly IFileLoader<Texture2D> _textureLoader;
   private readonly ILogger _logger;

   public CachedTextureLoader(IFileLoader<Texture2D> textureLoader, ILogger logger)
   {
      _textureLoader = textureLoader;
      _logger = logger;
   }

   public object Load(string textureFilePath)
       => new CachedTexture(textureFilePath, _textureLoader, _logger);
}
