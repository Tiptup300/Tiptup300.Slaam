using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SlaamMono.Resources.Loaders
{
    public class FontLoader : IFontLoader
    {
        public SpriteFont LoadFont(string directoryPath, string baseName)
        {
            string filePath = Path.Combine(directoryPath, baseName);
            SpriteFont temp = SlaamGame.Content.Load<SpriteFont>(filePath);
            return temp;
        }
    }
}
