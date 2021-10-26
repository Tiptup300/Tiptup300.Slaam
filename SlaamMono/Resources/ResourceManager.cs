using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Resources;
using SlaamMono.Resources.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Resources
{
    public class ResourceManager : IResources
    {
        public static ResourceManager Instance;

        private Dictionary<string, CachedTexture> _textures;
        private Dictionary<string, SpriteFont> _fonts;
        private Dictionary<string, string[]> _textLists;

        private ILogger _logger;
        private IResourceLoader _resourceLoader;

        public ResourceManager(
            ILogger logger,
            IResourceLoader resourceLoader)
        {
            _logger = logger;
            _resourceLoader = resourceLoader;

            Instance = this;
        }

        public CachedTexture GetTexture(string textureName) => _textures[textureName];
        public SpriteFont GetFont(string fontName) => _fonts[fontName];
        public List<string> GetTextList(string listName) => _textLists[listName].ToList();

        public void LoadAll()
        {
            _logger.Log("Resources Loading...");

            _textLists = loadTextLists();
            _logger.Log("Text Lists Loaded.");

            _textures = loadResource<CachedTexture>("Textures");
            _fonts = loadResource<SpriteFont>("Fonts");

            _logger.Log("All Resources Finished Loading;");
        }

        private Dictionary<string, string[]> loadTextLists()
        {
            Dictionary<string, string[]> output;

            output = new Dictionary<string, string[]>();
            output["BotNames"] = _resourceLoader.Load<string[]>("BotNames.txt");
            output["Credits"] = _resourceLoader.Load<string[]>("Credits.txt");
            output["Textures"] = _resourceLoader.Load<string[]>("Textures.txt");
            output["Fonts"] = _resourceLoader.Load<string[]>("Fonts.txt");

            return output;
        }

        private Dictionary<string, T> loadResource<T>(string listName) where T : class
        {
            dynamic output;

            output = _textLists[listName]
                .Select(line => line.Split(","))
                .ToDictionary(
                    x => x[0],
                    x => (T)_resourceLoader.Load<T>(x[1])
                );
            _logger.Log($"List \"{listName}\" of type \"{typeof(T).Name}\" Loaded.");

            return output;
        }
    }
}
