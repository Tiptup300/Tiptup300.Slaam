using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Input;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Metrics;
using SlaamMono.PlayerProfiles;
using SlaamMono.x_;
using ZBlade;

namespace SlaamMono
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SlaamGame : Game
    {
        new public static ContentManager Content;
        public static ZuneBlade mainBlade;


        private SpriteBatch gamebatch;

        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphicsState;
        private readonly RenderService _renderGraph;
        private readonly FpsRenderer _fpsRenderer;
        private readonly FrameTimeService _frameTimeService;
        private readonly InputService _inputService;

        public SlaamGame(
            ILogger logger,
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphicsState,
            RenderService renderGraph,
            FpsRenderer fpsRenderer,
            FrameTimeService frameTimeService,
            InputService inputService)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _graphicsState = graphicsState;
            _renderGraph = renderGraph;
            _fpsRenderer = fpsRenderer;
            _frameTimeService = frameTimeService;
            _inputService = inputService;


            Content = new ContentManager(Services);
            configureGame();
        }

        private void configureGame()
        {
            this.IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            _graphicsState.Set(new GraphicsDeviceManager(this)); // I'm not sure if I can move this line out to Initialize

            _inputService.Initialize();
            _renderGraph.Initialize();
            _fpsRenderer.Initialize();
            SetupZuneBlade();
            gamebatch = new SpriteBatch(_g.GraphicsDevice);
            _screenDirector.ChangeTo<ILogoScreen>();
        }

        protected override void LoadContent()
        {
            _logger.Log("Set Graphics Settings (1280x1024 No MultiSampling);");
            _resources.LoadAll();
            mainBlade.CurrentGameInfo.GameIcon = _resources.GetTexture("ZBladeGameIcon").Texture;
            Qwerty.CurrentPlayer = _inputService.GetPlayers()[0];
            _renderGraph.LoadContent();
            _fpsRenderer.LoadContent();
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
            _frameTimeService.Update(gameTime);
            _inputService.Update();
            if (ProfileManager.Initialized == false)
            {
                ProfileManager.Initialize(_logger, _resources);
            }
            else
            {
                _renderGraph.Update();
                _fpsRenderer.Update();

                if (Qwerty.Active)
                {
                    Qwerty.Update();
                }
                else
                {
                    _screenDirector.Update();
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _frameTimeService.Draw(gameTime);
            gamebatch.Begin(blendState: BlendState.NonPremultiplied);
            _screenDirector.Draw(gamebatch);

            if (Qwerty.Active)
            {
                Qwerty.Draw(gamebatch);
            }

            gamebatch.End();
            _renderGraph.Draw();
            _fpsRenderer.Draw();
        }
    }
}