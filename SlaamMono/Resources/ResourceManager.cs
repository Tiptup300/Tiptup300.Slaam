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

            return output;
        }
        private Dictionary<string, CachedTexture> loadTextures()
        {
            Dictionary<string, CachedTexture> output;

            output = new Dictionary<string, CachedTexture>();
            output["BattleBG"] = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/battlebg");
            output["ReadySetGo"] = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/readysetgo");
            output["RespawnTileOverlay"] = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/respawnOverlay");
            output["TileOverlay"] = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/tileOverlay");
            output["MenuTop"] = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/menutop");
            output["ProfileShell"] = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/CharacterSelectBox");
            output["StatsBoard"] = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/StatsScreen");
            output["LobbyCharBar"] = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/PlayerBar");
            output["LobbyUnderlay"] = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/LobbyBG");
            output["LobbyOverlay"] = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/LobbyOverlay");
            output["LobbyColorPreview"] = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/PlayerColorPreview");
            output["BoardSelect"] = _resourceLoader.Load<CachedTexture>("textures/Misc/boardSelect");
            output["ZibithLogoBG"] = _resourceLoader.Load<CachedTexture>("textures/Misc/LogoBG");
            output["ZibithLogo"] = _resourceLoader.Load<CachedTexture>("textures/Misc/Logo");
            output["NowLoading"] = _resourceLoader.Load<CachedTexture>("textures/Misc/BoardLoading");
            output["Background"] = _resourceLoader.Load<CachedTexture>("textures/Misc/background");
            output["FirstTime"] = _resourceLoader.Load<CachedTexture>("textures/firsttime");
            output["ZBladeGameIcon"] = _resourceLoader.Load<CachedTexture>("textures/Misc/ZBladeIcon");
            output["StatsButton1"] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton1");
            output["StatsButton2"] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton2");
            output["StatsButton3"] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton3");
            output["Inversion"] = _resourceLoader.Load<CachedTexture>("powerups\\Inversion");
            output["Inversion0"] = _resourceLoader.Load<CachedTexture>("powerups\\Inversion0");
            output["SpeedUp"] = _resourceLoader.Load<CachedTexture>("powerups\\SpeedUp");
            output["SpeedUp0"] = _resourceLoader.Load<CachedTexture>("powerups\\SpeedUp0");
            output["SpeedDown"] = _resourceLoader.Load<CachedTexture>("powerups\\SpeedDown");
            output["SpeedDown0"] = _resourceLoader.Load<CachedTexture>("powerups\\SpeedDown0");
            output["Slaam"] = _resourceLoader.Load<CachedTexture>("powerups\\Slaam");
            output["Slaam0"] = _resourceLoader.Load<CachedTexture>("powerups\\Slaam0");

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
