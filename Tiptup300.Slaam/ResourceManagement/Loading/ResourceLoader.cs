using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.ResourceManagement.Loading;

public class ResourceLoader : IResourceLoader
{
   private readonly string _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

   private readonly Dictionary<Type, IFileLoader> _fileLoaders;

   public ResourceLoader(
       IFileLoader<SpriteFont> fontLoader,
       IFileLoader<string[]> textLineLoader,
       IFileLoader<CachedTexture> cachedTextureFactory)
   {
      _fileLoaders = buildFileLoaders(fontLoader, textLineLoader, cachedTextureFactory);
   }

   public Dictionary<Type, IFileLoader> buildFileLoaders(
       IFileLoader<SpriteFont> fontLoader,
       IFileLoader<string[]> textLineLoader,
       IFileLoader<CachedTexture> cachedTextureFactory)
   {
      Dictionary<Type, IFileLoader> output;

      output = new Dictionary<Type, IFileLoader>
      {
         { typeof(SpriteFont), fontLoader },
         { typeof(string[]), textLineLoader },
         { typeof(CachedTexture), cachedTextureFactory }
      };

      return output;
   }

   public T Load<T>(string resourceName) where T : class
   {
      object output;
      string filePath;

      filePath = Path.Combine(_directoryPath, resourceName);
      output = _fileLoaders[typeof(T)].Load(filePath);

      return (T)output;
   }
}
