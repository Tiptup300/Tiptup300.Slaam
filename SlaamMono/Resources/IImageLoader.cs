using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources
{
    public interface IImageLoader
    {
        Texture2D LoadImage(string directoryPath, string baseName);
    }
}