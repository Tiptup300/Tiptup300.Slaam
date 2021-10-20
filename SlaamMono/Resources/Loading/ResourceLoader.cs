using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlaamMono.Resources.Loading
{
    public class ResourceLoader : IResourceLoader
    {
        private readonly IFontLoader _fontLoader;
        private readonly ITextLineLoader _textLineLoader;
        private readonly ICachedTextureFactory _cachedTextureFactory;

        private readonly string _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "content");

        public ResourceLoader(IFontLoader fontLoader, ITextLineLoader textLineLoader, ICachedTextureFactory cachedTextureFactory)
        {
            _fontLoader = fontLoader;
            _textLineLoader = textLineLoader;
            _cachedTextureFactory = cachedTextureFactory;
        }

        public T LoadResource<T>(string resourceName) where T : class
        {
            object output;
            string filePath;

            if (typeof(T) == typeof(CachedTexture))
            {
                filePath = Path.Combine(_directoryPath, "textures", "MOBILE", resourceName);
                output = _cachedTextureFactory.BuildCachedTexture(filePath);
            }
            else if (typeof(T) == typeof(IEnumerable<string>))
            {
                filePath = Path.Combine(_directoryPath, resourceName);
                output = _textLineLoader.LoadTextLines(filePath);
            }
            else if (typeof(T) == typeof(SpriteFont))
            {
                filePath = Path.Combine(_directoryPath, resourceName);
                output = _fontLoader.LoadFont(filePath);
            }
            else
            {
                throw new Exception("Resource Type not recognized!");
            }

            return (T)output;
        }
    }
}
