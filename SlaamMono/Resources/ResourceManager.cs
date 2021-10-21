using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Resources
{
    public static class ResourceManager
    {
        public static CachedTexture[] StatsButtons = new CachedTexture[3];
        // Text Files
        public static List<string> BotNames;
        public static List<string> Credits;
        // PowerUps 
        public static CachedTexture[] PU_SpeedUp = new CachedTexture[2];
        public static CachedTexture[] PU_SpeedDown = new CachedTexture[2];
        public static CachedTexture[] PU_Inversion = new CachedTexture[2];
        public static CachedTexture[] PU_Slaam = new CachedTexture[2];
        // General
        public static Texture2D WhitePixel;

        private static Dictionary<string, CachedTexture> _textures;
        private static Dictionary<string, SpriteFont> _fonts;

        private static ILogger _logger;
        private static IPixelFactory _pixelFactory;
        private static IResourceLoader _resourceLoader;
        private static ITextRenderer _textRenderer;

        public static void Initiailze(
            ILogger logger,
            IPixelFactory pixelFactory,
            IResourceLoader resourceLoader,
            ITextRenderer textRenderer)
        {
            _logger = logger;
            _pixelFactory = pixelFactory;
            _resourceLoader = resourceLoader;
            _textRenderer = textRenderer;
        }

        public static void LoadAll()
        {
            _logger.Log("Resources Loading...");

            WhitePixel = _pixelFactory.BuildPixel();
            _logger.Log(" - Dot Image Created.");


            _textures = loadTextures();
            _fonts = loadFonts();

            BotNames = _resourceLoader.Load<IEnumerable<string>>("BotNames.txt").ToList();
            Credits = _resourceLoader.Load<IEnumerable<string>>("Credits.txt").ToList();

            StatsButtons[0] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton1");
            StatsButtons[1] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton2");
            StatsButtons[2] = _resourceLoader.Load<CachedTexture>("MenuScreen/StatsButton3");

            loadPowerup(PU_SpeedUp, "SpeedUp");
            loadPowerup(PU_SpeedDown, "SpeedDown");
            loadPowerup(PU_Inversion, "Inversion");
            loadPowerup(PU_Slaam, "Slaam");

            _logger.Log("All Resources Finished Loading;");
        }

        private static Dictionary<string, SpriteFont> loadFonts()
        {
            Dictionary<string, SpriteFont> output;

            output = new Dictionary<string, SpriteFont>();
            output["SegoeUIx32pt"] = _resourceLoader.Load<SpriteFont>("SegoeUI-32pt");
            output["SegoeUIx14pt"] = _resourceLoader.Load<SpriteFont>("SegoeUI-14pt");
            output["SegoeUIx48ptBold"] = _resourceLoader.Load<SpriteFont>("SegoeUI-48pt");

            return output;
        }

        private static Dictionary<string, CachedTexture> loadTextures()
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

            return output;
        }

        public static CachedTexture GetTexture(string textureName)
        {
            return _textures[textureName];
        }

        public static SpriteFont GetFont(string fontName)
        {
            return _fonts[fontName];
        }

        private static void loadPowerup(CachedTexture[] Texs, string powerupname)
        {
            Texs[0] = _resourceLoader.Load<CachedTexture>("powerups\\" + powerupname);
            Texs[1] = _resourceLoader.Load<CachedTexture>("powerups\\" + powerupname + "0");
        }
    }
}
