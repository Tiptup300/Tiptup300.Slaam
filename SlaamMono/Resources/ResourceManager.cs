using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loaders;
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
        public static CachedTexture2D Feedbar;

        // BG
        public static CachedTexture2D Background;
        public static CachedTexture2D BattleBG;
        public static CachedTexture2D CreditsBG;


        // Qwerty
        public static CachedTexture2D Textbox;
        public static CachedTexture2D KeyboardBG;
        public static CachedTexture2D Key;
        public static CachedTexture2D Spacebar;
        public static CachedTexture2D KeyHT;
        public static CachedTexture2D SpaceHT;

        // Char Select
        public static CachedTexture2D ProfileShell;
        public static CachedTexture2D ProfileShello;

        // Lobby
        public static CachedTexture2D LobbyUnderlay;
        public static CachedTexture2D LobbyOverlay;
        public static CachedTexture2D LobbyColorPreview;
        public static CachedTexture2D LobbyCharBar;
        public static CachedTexture2D HostingBG;
        public static CachedTexture2D CPU;
        public static CachedTexture2D LButton;
        public static CachedTexture2D LButtonHT;
        public static CachedTexture2D LobbyInfoOverlay;
        public static CachedTexture2D BoardSelect;
        public static CachedTexture2D NowLoading;
        public static CachedTexture2D BoardSelectTextUnderlay;

        // Menu
        public static CachedTexture2D Gear;
        public static CachedTexture2D MenuTop;
        public static CachedTexture2D MenuBoard;
        public static CachedTexture2D MenuBlock;
        public static CachedTexture2D MenuOverlay;

        // Menu BG's
        public static CachedTexture2D Menu0;
        public static CachedTexture2D Menu1;
        public static CachedTexture2D Menu2;
        public static CachedTexture2D Menu3;
        public static CachedTexture2D Menu4;
        public static CachedTexture2D Menu5;
        public static CachedTexture2D Menu6;
        public static CachedTexture2D Menu7;

        // Game Board
        public static CachedTexture2D TileOverlay;
        public static CachedTexture2D RespawnTileOverlay;
        public static CachedTexture2D DeadChar;
        public static CachedTexture2D ReadySetGo;
        public static CachedTexture2D TopGameBoard;
        public static CachedTexture2D Waiting;
        public static CachedTexture2D GameScreenScoreBoard;
        public static CachedTexture2D PauseScreen;

        public static Texture2D DefaultBoard;

        // Stats
        public static CachedTexture2D StatsBoard;
        public static CachedTexture2D Star;
        public static Texture2D[] StatsButtons = new Texture2D[3];

        // Fonts
        public static TextManager textManager;
        public static SpriteFont SegoeUIx32pt;
        public static SpriteFont SegoeUIx14pt;
        public static SpriteFont SegoeUIx48ptBold;

        // Logos
        public static CachedTexture2D ZibithLogo;
        public static CachedTexture2D ZibithLogoBG;

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

        public static CachedTexture2D MenuChoice;
        public static CachedTexture2D MenuChoiceGlow;

        // PowerUps 
        public static Texture2D[] PU_SpeedUp = new Texture2D[2];
        public static Texture2D[] PU_SpeedDown = new Texture2D[2];
        public static Texture2D[] PU_Inversion = new Texture2D[2];
        public static Texture2D[] PU_Slaam = new Texture2D[2];

        private static ILogger _logger;
        private static IImageLoader _imageLoader;
        private static IPixelFactory _pixelFactory;
        private static ITextLineLoader _textLineLoader;
        private static IFontLoader _fontLoader;

        public static void Initiailze(
            ILogger logger, 
            IImageLoader imageLoader,
            IPixelFactory pixelFactory,
            ITextLineLoader textLineLoader,
            IFontLoader fontLoader)
        {
            _logger = logger;
            _imageLoader = imageLoader;
            _pixelFactory = pixelFactory;
            _textLineLoader = textLineLoader;
            _fontLoader = fontLoader;
        }

        public static void LoadAll()
        {
            _logger.Log("Resources Loading...");

            Dot = _pixelFactory.BuildPixel();
            _logger.Log(" - Dot Image Created.");


            BattleBG = new CachedTexture2D("BattleScreen//battlebg");
            ReadySetGo = new CachedTexture2D("BattleScreen//readysetgo");
            RespawnTileOverlay = new CachedTexture2D("BattleScreen//respawnOverlay");
            TileOverlay = new CachedTexture2D("BattleScreen//tileOverlay");

            MenuTop = new CachedTexture2D("MenuScreen//menutop");
            ProfileShell = new CachedTexture2D("MenuScreen//CharacterSelectBox");
            StatsBoard = new CachedTexture2D("MenuScreen/StatsScreen");
            StatsButtons[0] = LoadTexture("MenuScreen/StatsButton1");
            StatsButtons[1] = LoadTexture("MenuScreen/StatsButton2");
            StatsButtons[2] = LoadTexture("MenuScreen/StatsButton3");

            LobbyCharBar = new CachedTexture2D("LobbyScreen/PlayerBar");
            LobbyUnderlay = new CachedTexture2D("LobbyScreen/LobbyBG");
            LobbyOverlay = new CachedTexture2D("LobbyScreen/LobbyOverlay");
            LobbyColorPreview = new CachedTexture2D("LobbyScreen/PlayerColorPreview");

            BoardSelect = new CachedTexture2D("Misc//boardSelect");
            ZibithLogoBG = new CachedTexture2D("Misc//LogoBG");
            ZibithLogo = new CachedTexture2D("Misc//Logo");
            NowLoading = new CachedTexture2D("Misc//BoardLoading");
            Background = new CachedTexture2D("Misc//background");
            SlaamGame.mainBlade.CurrentGameInfo.GameIcon = LoadTexture("Misc//ZBladeIcon");

            SegoeUIx32pt = loadFont("SegoeUI-32pt");
            SegoeUIx14pt = loadFont("SegoeUI-14pt");
            SegoeUIx48ptBold = loadFont("SegoeUI-48pt");
            BotNames = loadStringList("BotNames");
            Credits = loadStringList("Credits");

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


        public static Texture2D LoadTexture(string name)
        {
            Texture2D output;
            string directoryPath;
            
            try
            {
                directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "content\\textures\\MOBILE\\");
                output = _imageLoader.LoadImage(directoryPath, name);
                _logger.Log($" - {name} Texture Loaded.");
            }
            catch (Exception ex)
            {
                output = _pixelFactory.BuildPixel();
                _logger.Log($"Texture \"{name}\" failed to load. Replaced with a blank pixel. Error: {ex.Message}");
            }

            return output;
        }

        private static List<string> loadStringList(string baseName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "content");
            return _textLineLoader.LoadTextLines(directoryPath, baseName)
                .ToList();
        }

        private static SpriteFont loadFont(string baseName)
        {
            SpriteFont output;

            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "content");
            output = _fontLoader.LoadFont(directoryPath, baseName);
            _logger.Log(" - " + baseName + " Font Loaded.");

            return output;
        }

        public static void DrawText(string text, Vector2 position, SpriteFont font, Color color, TextAlignment alignment, bool drawShadow)
        {
            textManager.AddTextToRender(text, position, font, color, alignment, drawShadow);
        }
    }
}
