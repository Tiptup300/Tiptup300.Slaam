using Microsoft.Xna.Framework.Graphics;

namespace Tiptup300.Slaam.Library.ResourceManagement;

public class ResourcesState
{
   public Dictionary<string, string[]> TextLists { get; private set; }
   public Dictionary<string, CachedTexture> Textures { get; private set; }
   public Dictionary<string, SpriteFont> Fonts { get; private set; }
   public string[] Boards { get; private set; }

   public ResourcesState(Dictionary<string, string[]> textLists, Dictionary<string, CachedTexture> textures, Dictionary<string, SpriteFont> fonts, string[] boards)
   {
      TextLists = textLists;
      Textures = textures;
      Fonts = fonts;
      Boards = boards;
   }
}
