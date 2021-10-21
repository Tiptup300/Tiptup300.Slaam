using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Resources
{
    public class ResourceManager
    {
        public static ResourceManager Instance;

        // Text Files
        public List<string> BotNames;
        public List<string> Credits;

        private Dictionary<string, CachedTexture> _textures;
        private Dictionary<string, SpriteFont> _fonts;

        private ILogger _logger;
        private IWhitePixelResolver _pixelFactory;
        private IResourceLoader _resourceLoader;

        public ResourceManager(
            ILogger logger,
            IWhitePixelResolver pixelFactory,
            IResourceLoader resourceLoader)
        {
            _logger = logger;
            _pixelFactory = pixelFactory;
            _resourceLoader = resourceLoader;

            Instance = this;
        }

        public void LoadAll()
        {
            _logger.Log("Resources Loading...");
            _textures = loadTextures();
            _logger.Log("Textures Loaded.");
            _fonts = loadFonts();
            _logger.Log("Fonts Loaded.");

            BotNames = _resourceLoader.Load<IEnumerable<string>>("BotNames.txt").ToList();
            Credits = _resourceLoader.Load<IEnumerable<string>>("Credits.txt").ToList();

            _logger.Log("All Resources Finished Loading;");
        }
        private void loadPowerup(CachedTexture[] Texs, string powerupname)
        {
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

        public CachedTexture GetTexture(string textureName)
        {
            return _textures[textureName];
        }

        public SpriteFont GetFont(string fontName)
        {
            return _fonts[fontName];
        }

    }
}
