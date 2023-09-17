using ZzziveGameEngine;

namespace SlaamMono.Library.ResourceManagement;

 public class TextureRequest : IRequest
 {
     public string TextureName { get; private set; }

     public TextureRequest(string textureName)
     {
         TextureName = textureName;
     }
 }
