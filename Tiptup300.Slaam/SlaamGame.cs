using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.Primitives;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.Graphics;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Metrics;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.ZibithLogo;

namespace Tiptup300.Slaam;

/// <summary>
/// This is the main type for your game
/// </summary>
public class SlaamGame : Game
{
   new public static ContentManager Content;

   private IState _state = new SlaamGameState();

   private readonly ILogger _logger;
   private readonly IResources _resources;
   private readonly IGraphicsStateService _graphicsState;
   private readonly RenderService _renderService;
   private readonly FpsRenderer _fpsRenderer;
   private readonly FrameTimeService _frameTimeService;
   private readonly InputService _inputService;
   private readonly IResolver<GameStartRequest, IState> _gameStartRequestResolver;
   private readonly ZibithLogoPerformer _performer;

   public SlaamGame(
       ILogger logger,
       IResources resources,
       IGraphicsStateService graphicsState,
       RenderService renderService,
       FpsRenderer fpsRenderer,
       FrameTimeService frameTimeService,
       InputService inputService,
       IResolver<GameStartRequest, IState> gameStartRequestResolver
   )
   {
      _logger = logger;
      _resources = resources;
      _graphicsState = graphicsState;
      _renderService = renderService;
      _fpsRenderer = fpsRenderer;
      _frameTimeService = frameTimeService;
      _inputService = inputService;
      _gameStartRequestResolver = gameStartRequestResolver;

      Content = new ContentManager(Services);
      _graphicsState.Set(new GraphicsDeviceManager(this)); // I'm not sure if I can move this line out to Initialize
                                                           //   _performer = new ZibithLogoPerformer(resources, frameTimeService, renderService);
   }

   private void configureGame()
   {
      IsFixedTimeStep = false;
   }

   protected override void Initialize()
   {
      configureGame();

      _logger.Initialize();
      _inputService.Initialize();
      _renderService.Initialize();
      _fpsRenderer.Initialize();

      base.Initialize();
   }

   protected override void LoadContent()
   {
      _resources.LoadAll();
      _renderService.LoadContent();
      _fpsRenderer.LoadContent();

      base.LoadContent();
   }

   protected override void Update(GameTime gameTime)
   {
      if (_state is null)
      {
         _state = _gameStartRequestResolver.Resolve(new GameStartRequest());
      }
      _frameTimeService.AddUpdate(gameTime);
      _inputService.Update();
      if (ProfileManager.Initialized == false)
      {
         ProfileManager.Initialize(_logger, _resources);
      }
      else
      {
         _renderService.Update();
         _fpsRenderer.Update();

         // update the state using the state performer.
         //   _state = _statePerformer.Resolve(_state;// update state

      }

      base.Update(gameTime);
   }
   protected override void Draw(GameTime gameTime)
   {
      _frameTimeService.AddDraw(gameTime);

      GraphicsDevice.Clear(Color.Black);



      // draw the state using the state renderer.
      // where in the world is the state renderer?
      // _screenDirector.Draw(gamebatch);

      _renderService.Draw();
      _fpsRenderer.Draw();

      base.Draw(gameTime);
   }
}