using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Drawing.Text;
using SlaamMono.Library.Logging;
using SlaamMono.Resources.Loading;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Resources
{
    public static class ResourceManager
    {

        // General
        public static Texture2D WhitePixel;

        private static Dictionary<string, CachedTexture> _textures;

        public static CachedTexture Feedbar;
        public static CachedTexture FirstTime;
        public static CachedTexture ZBladeGameIcon;
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
        // Stats
        public static CachedTexture StatsBoard;
        public static CachedTexture Star;
        public static CachedTexture[] StatsButtons = new CachedTexture[3];
        // Fonts
        public static SpriteFont SegoeUIx32pt;
        public static SpriteFont SegoeUIx14pt;
        public static SpriteFont SegoeUIx48ptBold;
        // Text Files
        public static List<string> BotNames;
        public static List<string> Credits;
        // Menu
        public static CachedTexture MenuChoice;
        public static CachedTexture MenuChoiceGlow;
        // PowerUps 
        public static CachedTexture[] PU_SpeedUp = new CachedTexture[2];
        public static CachedTexture[] PU_SpeedDown = new CachedTexture[2];
        public static CachedTexture[] PU_Inversion = new CachedTexture[2];
        public static CachedTexture[] PU_Slaam = new CachedTexture[2];

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

            BattleBG = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/battlebg");
            ReadySetGo = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/readysetgo");
            RespawnTileOverlay = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/respawnOverlay");
            TileOverlay = _resourceLoader.Load<CachedTexture>("textures/BattleScreen/tileOverlay");
            MenuTop = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/menutop");
            ProfileShell = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/CharacterSelectBox");
            StatsBoard = _resourceLoader.Load<CachedTexture>("textures/MenuScreen/StatsScreen");
            LobbyCharBar = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/PlayerBar");
            LobbyUnderlay = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/LobbyBG");
            LobbyOverlay = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/LobbyOverlay");
            LobbyColorPreview = _resourceLoader.Load<CachedTexture>("textures/LobbyScreen/PlayerColorPreview");
            BoardSelect = _resourceLoader.Load<CachedTexture>("textures/Misc/boardSelect");
            NowLoading = _resourceLoader.Load<CachedTexture>("textures/Misc/BoardLoading");
            Background = _resourceLoader.Load<CachedTexture>("textures/Misc/background");
            FirstTime = _resourceLoader.Load<CachedTexture>("textures/firsttime");
            ZBladeGameIcon = _resourceLoader.Load<CachedTexture>("textures/Misc/ZBladeIcon");

            _textures = loadTextures();

            SegoeUIx32pt = _resourceLoader.Load<SpriteFont>("SegoeUI-32pt");
            SegoeUIx14pt = _resourceLoader.Load<SpriteFont>("SegoeUI-14pt");
            SegoeUIx48ptBold = _resourceLoader.Load<SpriteFont>("SegoeUI-48pt");

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

        private static void loadPowerup(CachedTexture[] Texs, string powerupname)
        {
            Texs[0] = _resourceLoader.Load<CachedTexture>("powerups\\" + powerupname);
            Texs[1] = _resourceLoader.Load<CachedTexture>("powerups\\" + powerupname + "0");
        }
    }
}
