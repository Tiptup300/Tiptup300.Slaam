using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam;
using Tiptup300.Slaam.Library.ResourceManagement;

namespace Tiptup300.Slaam.ResourceManagement.Loading;

public class FontLoader : IFileLoader<SpriteFont>
{
   public object Load(string filePath)
   {
      SpriteFont output;

      output = SlaamGame.Content.Load<SpriteFont>(filePath);

      return output;
   }
}
