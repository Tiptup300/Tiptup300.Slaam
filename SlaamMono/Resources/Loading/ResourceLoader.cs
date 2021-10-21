using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlaamMono.Resources.Loading
{
    public class ResourceLoader : IResourceLoader
    {
        private readonly string _directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "content");

        private readonly Dictionary<Type, IFileLoader> _fileLoaders;

        public ResourceLoader(
            IFileLoader<SpriteFont> fontLoader,
            IFileLoader<string[]> textLineLoader,
            IFileLoader<CachedTexture> cachedTextureFactory)
        {
            _fileLoaders = buildFileLoaders(fontLoader, textLineLoader, cachedTextureFactory);
        }

        public Dictionary<Type, IFileLoader> buildFileLoaders(
            IFileLoader<SpriteFont> fontLoader,
            IFileLoader<string[]> textLineLoader,
            IFileLoader<CachedTexture> cachedTextureFactory)
        {
            Dictionary<Type, IFileLoader> output;

            output = new Dictionary<Type, IFileLoader>();
            output.Add(typeof(SpriteFont), fontLoader);
            output.Add(typeof(string[]), textLineLoader);
            output.Add(typeof(CachedTexture), cachedTextureFactory);

            return output;
        }

        public T Load<T>(string resourceName) where T : class
        {
            object output;
            string filePath;

            filePath = Path.Combine(_directoryPath, resourceName);
            output = _fileLoaders[typeof(T)].Load(filePath);

            return (T)output;
        }
    }
}
