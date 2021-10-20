using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources
{
    public interface IPixelFactory
    {
        Texture2D BuildPixel();
    }
}