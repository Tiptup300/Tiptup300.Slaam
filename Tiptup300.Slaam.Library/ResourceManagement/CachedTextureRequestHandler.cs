using System.Tiptup300.Primitives;

namespace Tiptup300.Slaam.Library.ResourceManagement;

public class CachedTextureRequestHandler : IResolver<TextureRequest, CachedTexture>
{
   private readonly Mut<ResourcesState> _resourcesState;

   public CachedTextureRequestHandler(Mut<ResourcesState> resourcesState)
   {
      _resourcesState = resourcesState;
   }

   public CachedTexture Resolve(TextureRequest request)
   {
      CachedTexture output;

      output = _resourcesState.Get().Textures[request.TextureName];
      output.Load();

      return output;
   }
}
