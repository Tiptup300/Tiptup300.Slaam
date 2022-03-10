using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Input;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Metrics;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.PlayerProfiles;
using SlaamMono.x_;
using ZBlade;

namespace SlaamMono
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SlaamGame : Game, ISlaamGame
    {
        private GraphicsDeviceManager _graphics;
        new public static ContentManager Content;

        public static ZuneBlade mainBlade;
        public static SlaamGame Instance { get { return instance; } }
        public static SlaamGame instance;
        public static bool ShowFPS = true;

        private SpriteBatch gamebatch;

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraph;
        private readonly IFpsRenderer _fpsRenderer;

        public SlaamGame(
            ILogger logger,
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphicsState,
            IRenderGraph renderGraph,
            IFpsRenderer fpsRenderer)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _renderGraph = renderGraph;
            _fpsRenderer = fpsRenderer;

            _graphics = new GraphicsDeviceManager(this);
            graphicsState.Set(_graphics);
            Content = new ContentManager(Services);
            this.IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            Components.Insert(0, new FrameRateDirector(this));
            Components.Add(new InputComponent(this));
            _renderGraph.Initialize();
            _fpsRenderer.Initialize();
            SetupZuneBlade();
            gamebatch = new SpriteBatch(_graphics.GraphicsDevice);
            instance = this;
            _screenDirector.ChangeTo<ILogoScreen>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Log("Set Graphics Settings (1280x1024 No MultiSampling);");
            _resources.LoadAll();
            SlaamGame.mainBlade.CurrentGameInfo.GameIcon = _resources.GetTexture("ZBladeGameIcon").Texture;
            Qwerty.CurrentPlayer = InputComponent.Players[0];
            _renderGraph.LoadContent();
            _fpsRenderer.LoadContent();

            base.LoadContent();
        }

        public void SetupZuneBlade()
        {
            mainBlade = new ZuneBlade(this);

            mainBlade.Opacity = 1f;
            mainBlade.CurrentGameInfo = new GameInfo("Slaam! Mobile", "Zibith Games", null);
            mainBlade.UserCanNavigateMenu = true;
            mainBlade.UserCanCloseMenu = false;
            mainBlade.Status = BladeStatus.Hidden;
            mainBlade.ScreenWidth = GameGlobals.DRAWING_GAME_WIDTH;
            mainBlade.ScreenHeight = GameGlobals.DRAWING_GAME_HEIGHT;

            Components.Add(mainBlade);
        }
        protected override void Update(GameTime gameTime)
        {

            if (ProfileManager.Initialized == false)
            {
                ProfileManager.Initialize(_logger, _resources);
            }
            else
            {

                _renderGraph.Update(gameTime);
                _fpsRenderer.Update(gameTime);

                if (Qwerty.Active)
                {
                    Qwerty.Update();
                }
                else
                {
                    _screenDirector.Update();
                }
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            gamebatch.Begin(blendState: BlendState.NonPremultiplied);
            _screenDirector.Draw(gamebatch);

            if (Qwerty.Active)
            {
                Qwerty.Draw(gamebatch);
            }

            gamebatch.End();
            _renderGraph.Draw();
            _fpsRenderer.Draw();

            base.Draw(gameTime);
        }

        public Game Game => this;
    }
}