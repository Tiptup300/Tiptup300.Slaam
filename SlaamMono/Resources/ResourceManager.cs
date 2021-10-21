using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Resources
{
    public class ResourceManager
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

            _textures = loadTextures();
            _logger.Log("Textures Loaded.");

            _fonts = loadFonts();
            _logger.Log("Fonts Loaded.");

            _logger.Log("All Resources Finished Loading;");
        }
        private Dictionary<string, string[]> loadTextLists()
        {
            Dictionary<string, string[]> output;

            output = new Dictionary<string, string[]>();
            output["BotNames"] = _resourceLoader.Load<string[]>("BotNames.txt");
            output["Credits"] = _resourceLoader.Load<string[]>("Credits.txt");
            output["Textures"] = _resourceLoader.Load<string[]>("Textures.txt");

            return output;
        }
        private Dictionary<string, CachedTexture> loadTextures()
        {
            Dictionary<string, CachedTexture> output;

            output = _textLists["Textures"]
                .Select(line => line.Split(","))
                .ToDictionary(
                    x => x[0],
                    x => _resourceLoader.Load<CachedTexture>(x[1])
                );

            return output;
        }
        private Dictionary<string, SpriteFont> loadFonts()
        {
            Dictionary<string, SpriteFont> output;

            output = new Dictionary<string, SpriteFont>();
            output["SegoeUIx32pt"] = _resourceLoader.Load<SpriteFont>("SegoeUI-32pt");
            output["SegoeUIx14pt"] = _resourceLoader.Load<SpriteFont>("SegoeUI-14pt");
            output["SegoeUIx48ptBold"] = _resourceLoader.Load<SpriteFont>("SegoeUI-48pt");

            return output;
        }





    }
}
