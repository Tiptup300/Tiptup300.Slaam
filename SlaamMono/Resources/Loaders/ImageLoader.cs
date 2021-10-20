using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using System.IO;

namespace SlaamMono.Resources.Loaders
{
    public class ImageLoader : IImageLoader
    {
        private ILogger _logger;
        private readonly IPixelFactory _pixelFactory;

        public ImageLoader(ILogger logger, IPixelFactory pixelFactory)
        {
            _logger = logger;
            _pixelFactory = pixelFactory;
        }

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
