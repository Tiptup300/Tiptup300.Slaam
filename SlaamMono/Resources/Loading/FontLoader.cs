using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SlaamMono.Resources.Loading
{
    public class FontLoader : IFileLoader<SpriteFont>
    {
        public object Load(string filePath)
        {
            SpriteFont output;
            
            output = SlaamGame.Content.Load<SpriteFont>(filePath);

            return output;
        }
    }
}
