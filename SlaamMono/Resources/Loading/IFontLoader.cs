using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources.Loading
{
    public interface IFontLoader
    {
        SpriteFont LoadFont(string filePath);
    }
}