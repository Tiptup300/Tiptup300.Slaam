using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SlaamMono.Resources.Loading
{
    public class FontLoader : IFontLoader
    {
        public SpriteFont LoadFont(string filePath)
        {
            SpriteFont temp = SlaamGame.Content.Load<SpriteFont>(filePath);
            return temp;
        }
    }
}
