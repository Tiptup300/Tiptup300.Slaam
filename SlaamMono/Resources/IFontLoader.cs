using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources
{
    public interface IFontLoader
    {
        SpriteFont LoadFont(string directoryPath, string baseName);
    }
}