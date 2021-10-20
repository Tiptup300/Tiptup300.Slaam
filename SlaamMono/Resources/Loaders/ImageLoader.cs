using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using System.IO;

namespace SlaamMono.Resources.Loaders
{
    public class ImageLoader : IImageLoader
    {

        public Texture2D LoadImage(string directoryPath, string baseName)
        {
            Texture2D output;
            string filePath;

            filePath = Path.Combine(directoryPath, baseName);
            output = SlaamGame.Content.Load<Texture2D>(filePath);

            return output;
        }
    }
}
