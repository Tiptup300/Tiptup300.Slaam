using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement.Loading;
using System.Collections.Generic;
using System.Linq;
using ZzziveGameEngine;

namespace SlaamMono.ResourceManagement
{
    public class Resources : IResources
    {
        private readonly ILogger _logger;
        private readonly IResourceLoader _resourceLoader;
        private readonly Mut<ResourcesState> _state;
        private readonly ResourcesListsToLoad _resourcesListsToLoad;

        public Resources(
            ILogger logger,
            IResourceLoader resourceLoader,
            Mut<ResourcesState> resourcesState,
            ResourcesListsToLoad resourcesListsToLoad)
        {
            _logger = logger;
            _resourceLoader = resourceLoader;
            _state = resourcesState;
            _resourcesListsToLoad = resourcesListsToLoad;
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

            var boards = textLists["BoardList"];

            _state.Mutate(new ResourcesState(textLists, textures, fonts, boards));

            _logger.Log("All Resources Finished Loading;");
        }

        private Dictionary<string, string[]> loadTextLists()
        {
            Dictionary<string, string[]> output;

            output = _resourcesListsToLoad.TextLists.ToDictionary(
                listName => listName,
                listName => _resourceLoader.Load<string[]>($"{listName}.txt"));

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
            => _state.Get().Textures[textureName];

        public SpriteFont GetFont(string fontName)
            => _state.Get().Fonts[fontName];

        public List<string> GetTextList(string listName)
            => _state.Get().TextLists[listName]
                .ToList();
    }
}
