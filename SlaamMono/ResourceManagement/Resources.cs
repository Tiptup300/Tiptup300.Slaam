using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.ResourceManagement
{
    public class Resources : IResources
    {
        public static Resources Instance;

        private readonly ResourcesState _state;
        private readonly ILogger _logger;
        private readonly IResourceLoader _resourceLoader;

        public Resources(
            ILogger logger,
            IResourceLoader resourceLoader, ResourcesState resourcesState)
        {
            _logger = logger;
            _resourceLoader = resourceLoader;
            _state = resourcesState;
            Instance = this;
        }

        public void LoadAll()
        {
            _logger.Log("Resources Loading...");

            _state.TextLists = loadTextLists();
            _logger.Log("Text Lists Loaded.");

            _state.Textures = loadResource<CachedTexture>("Textures");
            _logger.Log($"Textures Loaded.");

            _state.Fonts = loadResource<SpriteFont>("Fonts");
            _logger.Log($"Fonts Loaded.");

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
            return _state.TextLists[listName]
                .Select(line => line.Split(","))
                .ToDictionary(
                    x => x[0],
                    x => (T)_resourceLoader.Load<T>(x[1])
                );
        }

        public CachedTexture GetTexture(string textureName)
            => _state.Textures[textureName];

        public SpriteFont GetFont(string fontName)
            => _state.Fonts[fontName];

        public List<string> GetTextList(string listName)
            => _state.TextLists[listName]
                .ToList();
    }
}
