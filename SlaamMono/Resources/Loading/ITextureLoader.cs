using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources.Loading
{
    public interface ITextureLoader
    {
        Texture2D Load(string filePath);
    }
}