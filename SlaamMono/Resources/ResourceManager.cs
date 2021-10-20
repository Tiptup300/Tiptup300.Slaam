using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.Resources
{
    public static class ResourceManager
    {
        // General
        public static Texture2D Dot;
        public static CachedTexture Feedbar;

        // BG
        public static CachedTexture Background;
        public static CachedTexture BattleBG;
        public static CachedTexture CreditsBG;

        // Qwerty
        public static CachedTexture Textbox;
        public static CachedTexture KeyboardBG;
        public static CachedTexture Key;
        public static CachedTexture Spacebar;
        public static CachedTexture KeyHT;
        public static CachedTexture SpaceHT;

        // Char Select
        public static CachedTexture ProfileShell;
        public static CachedTexture ProfileShello;

        // Lobby
        public static CachedTexture LobbyUnderlay;
        public static CachedTexture LobbyOverlay;
        public static CachedTexture LobbyColorPreview;
        public static CachedTexture LobbyCharBar;
        public static CachedTexture HostingBG;
        public static CachedTexture CPU;
        public static CachedTexture LButton;
        public static CachedTexture LButtonHT;
        public static CachedTexture LobbyInfoOverlay;
        public static CachedTexture BoardSelect;
        public static CachedTexture NowLoading;
        public static CachedTexture BoardSelectTextUnderlay;

        // Menu
        public static CachedTexture Gear;
        public static CachedTexture MenuTop;
        public static CachedTexture MenuBoard;
        public static CachedTexture MenuBlock;
        public static CachedTexture MenuOverlay;

        // Menu BG's
        public static CachedTexture Menu0;
        public static CachedTexture Menu1;
        public static CachedTexture Menu2;
        public static CachedTexture Menu3;
        public static CachedTexture Menu4;
        public static CachedTexture Menu5;
        public static CachedTexture Menu6;
        public static CachedTexture Menu7;

        // Game Board
        public static CachedTexture TileOverlay;
        public static CachedTexture RespawnTileOverlay;
        public static CachedTexture DeadChar;
        public static CachedTexture ReadySetGo;
        public static CachedTexture TopGameBoard;
        public static CachedTexture Waiting;
        public static CachedTexture GameScreenScoreBoard;
        public static CachedTexture PauseScreen;

        public static CachedTexture ZBladeGameIcon;

        public static Texture2D DefaultBoard;

        // Stats
        public static CachedTexture StatsBoard;
        public static CachedTexture Star;
        public static Texture2D[] StatsButtons = new Texture2D[3];

        // Fonts
        public static TextManager textManager;
        public static SpriteFont SegoeUIx32pt;
        public static SpriteFont SegoeUIx14pt;
        public static SpriteFont SegoeUIx48ptBold;

        // Logos
        public static CachedTexture ZibithLogo;
        public static CachedTexture ZibithLogoBG;

        // Text Files
        public static List<string> BotNames;
        public static List<string> Credits;

        // Player Colors
        public static Color[] PlayerColors = new Color[] {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Cyan,
            Color.Orange,
            Color.Purple,
            Color.Pink
        };

        public static CachedTexture MenuChoice;
        public static CachedTexture MenuChoiceGlow;

        // PowerUps 
        public static Texture2D[] PU_SpeedUp = new Texture2D[2];
        public static Texture2D[] PU_SpeedDown = new Texture2D[2];
        public static Texture2D[] PU_Inversion = new Texture2D[2];
        public static Texture2D[] PU_Slaam = new Texture2D[2];

        private static ILogger _logger;
        private static ITextureLoader _imageLoader;
        private static IPixelFactory _pixelFactory;
        private static IResourceLoader _resourceLoader;

        public static void Initiailze(
            ILogger logger, 
            ITextureLoader textureLoader,
            IPixelFactory pixelFactory,
            IResourceLoader resourceLoader)
        {
            _logger = logger;
            _imageLoader = textureLoader;
            _pixelFactory = pixelFactory;
            _resourceLoader = resourceLoader;
        }

        public static void LoadAll()
        {
            _logger.Log("Resources Loading...");

            Dot = _pixelFactory.BuildPixel();
            _logger.Log(" - Dot Image Created.");

            BattleBG = _resourceLoader.LoadResource<CachedTexture>("BattleScreen/battlebg");
            ReadySetGo = _resourceLoader.LoadResource<CachedTexture>("BattleScreen/readysetgo");
            RespawnTileOverlay = _resourceLoader.LoadResource<CachedTexture>("BattleScreen/respawnOverlay");
            TileOverlay = _resourceLoader.LoadResource<CachedTexture>("BattleScreen/tileOverlay");
            MenuTop = _resourceLoader.LoadResource<CachedTexture>("MenuScreen/menutop");
            ProfileShell = _resourceLoader.LoadResource<CachedTexture>("MenuScreen/CharacterSelectBox");
            StatsBoard = _resourceLoader.LoadResource<CachedTexture>("MenuScreen/StatsScreen");
            LobbyCharBar = _resourceLoader.LoadResource<CachedTexture>("LobbyScreen/PlayerBar");
            LobbyUnderlay = _resourceLoader.LoadResource<CachedTexture>("LobbyScreen/LobbyBG");
            LobbyOverlay = _resourceLoader.LoadResource<CachedTexture>("LobbyScreen/LobbyOverlay");
            LobbyColorPreview = _resourceLoader.LoadResource<CachedTexture>("LobbyScreen/PlayerColorPreview");
            BoardSelect = _resourceLoader.LoadResource<CachedTexture>("Misc/boardSelect");
            ZibithLogoBG = _resourceLoader.LoadResource<CachedTexture>("Misc/LogoBG");
            ZibithLogo = _resourceLoader.LoadResource<CachedTexture>("Misc/Logo");
            NowLoading = _resourceLoader.LoadResource<CachedTexture>("Misc/BoardLoading");
            Background = _resourceLoader.LoadResource<CachedTexture>("Misc/background");
            ZBladeGameIcon = _resourceLoader.LoadResource<CachedTexture>("Misc/ZBladeIcon");

            StatsButtons[0] = LoadTexture("MenuScreen/StatsButton1");
            StatsButtons[1] = LoadTexture("MenuScreen/StatsButton2");
            StatsButtons[2] = LoadTexture("MenuScreen/StatsButton3");

            SegoeUIx32pt = _resourceLoader.LoadResource<SpriteFont>("SegoeUI-32pt");
            SegoeUIx14pt = _resourceLoader.LoadResource<SpriteFont>("SegoeUI-14pt");
            SegoeUIx48ptBold = _resourceLoader.LoadResource<SpriteFont>("SegoeUI-48pt");
            BotNames = _resourceLoader.LoadResource<IEnumerable<String>>("BotNames.txt").ToList();
            Credits = _resourceLoader.LoadResource<IEnumerable<String>>("Credits.txt").ToList();

            loadPowerup(PU_SpeedUp, "SpeedUp");
            loadPowerup(PU_SpeedDown, "SpeedDown");
            loadPowerup(PU_Inversion, "Inversion");
            loadPowerup(PU_Slaam, "Slaam");

            _logger.Log("All Resources Finished Loading;");
            textManager = new TextManager(SlaamGame.Instance);
            SlaamGame.Instance.Components.Add(textManager);
        }

        private static void loadPowerup(Texture2D[] Texs, string powerupname)
        {
            Texs[0] = LoadTexture("powerups\\" + powerupname);
            Texs[1] = LoadTexture("powerups\\" + powerupname + "0");
        }


        public static Texture2D LoadTexture(string textureName)
        {
            Texture2D output;
            
            try
            {
                string filePath;

                filePath = Path.Combine(Directory.GetCurrentDirectory(), "content\\textures\\MOBILE\\", textureName);
                output = _imageLoader.LoadImage(filePath);
                _logger.Log($" - {textureName} Texture Loaded.");
            }
            catch (Exception ex)
            {
                output = _pixelFactory.BuildPixel();
                _logger.Log($"Texture \"{textureName}\" failed to load. Replaced with a blank pixel. Error: {ex.Message}");
            }

            return output;
        }

        public static void DrawText(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment, bool drawShadow)
        {
            textManager.AddTextToRender(text, position, font, color, alignment, drawShadow);
        }
    }
}
