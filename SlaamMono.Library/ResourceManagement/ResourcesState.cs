using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Library.ResourceManagement
{
    public class ResourcesState
    {
        public Dictionary<string, string[]> TextLists { get; set; }
        public Dictionary<string, CachedTexture> Textures { get; set; }
        public Dictionary<string, SpriteFont> Fonts { get; set; }
    }
}
