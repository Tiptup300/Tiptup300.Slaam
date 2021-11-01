using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Library.ResourceManagement
{
    public class ResourcesState
    {
        public Dictionary<string, string[]> TextLists { get; private set; }
        public Dictionary<string, CachedTexture> Textures { get; private set; }
        public Dictionary<string, SpriteFont> Fonts { get; private set; }

        public ResourcesState(Dictionary<string, string[]> textLists, Dictionary<string, CachedTexture> textures, Dictionary<string, SpriteFont> fonts)
        {
            TextLists = textLists;
            Textures = textures;
            Fonts = fonts;
        }
    }
}
