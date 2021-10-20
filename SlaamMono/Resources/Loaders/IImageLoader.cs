using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources.Loaders
{
    public interface IImageLoader
    {
        Texture2D LoadImage(string directoryPath, string baseName);
    }
}