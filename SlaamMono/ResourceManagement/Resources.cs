using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.ResourceManagement
{
    public class Resources : IResources
    {
        private readonly ILogger _logger;
        private readonly IResourceLoader _resourceLoader;
        private readonly StateChanger<ResourcesState> _state;

        public Resources(
            ILogger logger,
            IResourceLoader resourceLoader,
            StateChanger<ResourcesState> resourcesState)
        {
            _logger = logger;
            _resourceLoader = resourceLoader;
            _state = resourcesState;
        }

        public void LoadAll()
        {
            _logger.Log("Resources Loading...");

            var textLists = loadTextLists();
            _logger.Log("Text Lists Loaded.");

            var textures = loadResource<CachedTexture>(textLists["Textures"]);
            _logger.Log($"Textures Loaded.");

            var fonts = loadResource<SpriteFont>(textLists["Fonts"]);
            _logger.Log($"Fonts Loaded.");

            _state.ChangeState(new ResourcesState(textLists, textures, fonts));

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

        private Dictionary<string, T> loadResource<T>(string[] textList) where T : class
        {
            return textList
                .Select(line => line.Split(","))
                .ToDictionary(
                    x => x[0],
                    x => (T)_resourceLoader.Load<T>(x[1])
                );
        }

        public CachedTexture GetTexture(string textureName)
            => _state.State.Textures[textureName];

        public SpriteFont GetFont(string fontName)
            => _state.State.Fonts[fontName];

        public List<string> GetTextList(string listName)
            => _state.State.TextLists[listName]
                .ToList();
    }
}
