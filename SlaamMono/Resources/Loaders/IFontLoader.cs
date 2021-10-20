using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources.Loaders
{
    public interface IFontLoader
    {
        SpriteFont LoadFont(string directoryPath, string baseName);
    }
}