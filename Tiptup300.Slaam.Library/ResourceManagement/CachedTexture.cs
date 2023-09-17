using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Logging;

namespace Tiptup300.Slaam.Library.ResourceManagement;

public class CachedTexture
{
   public Texture2D Texture
   {
      get
      {
         if (_isUnloaded)
         {
            _logger.Log($"Texture was accessed without preloading. Possible performance issue. (Filepath: {_filePath})");
            loadTexture();
         }
         return _texture;
      }
      set
      {
         _texture = value;
      }
   }
   public int Height { get { return Texture.Height; } }
   public int Width { get { return Texture.Width; } }

   private string _filePath;
   private Texture2D _texture;

   private readonly IFileLoader<Texture2D> _textureLoader;
   private readonly ILogger _logger;


   public CachedTexture(string filePath, IFileLoader<Texture2D> textureLoader, ILogger logger)
   {
      _filePath = filePath;
      _textureLoader = textureLoader;
      _logger = logger;
   }

   private bool _isUnloaded => _texture == null;


   public void Unload()
   {
      if (_isUnloaded)
      {
         return;
      }

      _texture.Dispose();
      _texture = null;
   }

   public void Load()
   {
      loadTexture();
   }

   private void loadTexture()
   {
      _texture = (Texture2D)_textureLoader.Load(_filePath);
   }

}