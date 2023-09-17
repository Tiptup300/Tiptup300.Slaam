using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tiptup300.Slaam.Library.ResourceManagement;

public interface IResources
{
   SpriteFont GetFont(string fontName);
   List<string> GetTextList(string listName);
   CachedTexture GetTexture(string textureName);
   void LoadAll();
}