using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using System;
using ZzziveGameEngine;

namespace SlaamMono.ResourceManagement.Loading
{
    public class Texture2DLoader : IFileLoader<Texture2D>
    {
        private readonly ILogger _logger;
        private readonly IResolver<WhitePixelRequest, Texture2D> _pixelFactory;

        public Texture2DLoader(ILogger logger, IResolver<WhitePixelRequest, Texture2D> pixelFactory)
        {
            _logger = logger;
            _pixelFactory = pixelFactory;
        }

        public object Load(string filePath)
        {
            Texture2D output;

            try
            {
                output = SlaamGame.Content.Load<Texture2D>(filePath);
                _logger.Log($" - {filePath} Texture Loaded.");
            }
            catch (Exception ex)
            {
                output = _pixelFactory.Resolve(new WhitePixelRequest());
                _logger.Log($"Texture \"{filePath}\" failed to load. Replaced with a blank pixel. Error: {ex.Message}");
            }
            return output;
        }
    }
}
