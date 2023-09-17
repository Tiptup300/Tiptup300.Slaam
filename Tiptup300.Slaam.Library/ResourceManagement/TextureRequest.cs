using System.Tiptup300.Primitives;

namespace Tiptup300.Slaam.Library.ResourceManagement;

public class TextureRequest : IRequest
{
   public string TextureName { get; private set; }

   public TextureRequest(string textureName)
   {
      TextureName = textureName;
   }
}
