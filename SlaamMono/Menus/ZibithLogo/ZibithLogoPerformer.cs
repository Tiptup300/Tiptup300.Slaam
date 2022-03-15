using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Subclasses;
using SlaamMono.x_;
using System;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus.ZibithLogo
{
    public class ZibithLogoPerformer : IPerformer<ZibithLogoState>
    {
        private const float _fadeInSeconds = 1f;
        private const float _holdSeconds = 3f;
        private const float _fadeOutSeconds = 0.5f;

        private readonly IResources _resources;
        private readonly IFrameTimeService _frameTimeService;
        private readonly IRenderService _renderService;

        public ZibithLogoPerformer(
            IResources resources,
            IFrameTimeService frameTimeService,
            IRenderService renderService)
        {
            _resources = resources;
            _frameTimeService = frameTimeService;
            _renderService = renderService;
        }

        public void InitializeState()
        {
            // to remove
        }

        public IState Perform()
        {
            ZibithLogoState _state = null; //TODO
            switch (_state.StateIndex)
            {
                default: return initFadeInState(_state);
                case 1: return fadeInState(_state);
                case 2: return holdState(_state);
                case 3: return fadeOutState(_state);
            }
        }

        private static IState initFadeInState(ZibithLogoState state)
        {
            state.StateIndex = 1;
            state.StateTransition = new Transition(TimeSpan.FromSeconds(_fadeInSeconds));
            state.LogoColor = new Color(Color.White, 0);

            return state;
        }

        private IState fadeInState(ZibithLogoState state)
        {
            state.StateTransition.AddProgress(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(0f, 1f, state.StateTransition.Position));
            if (state.StateTransition.IsFinished)
            {
                return initHoldState(state);
            }

            return state;
        }

        private static IState initHoldState(ZibithLogoState state)
        {
            state.StateIndex = 2;
            state.LogoColor = Color.White;
            state.StateTransition.Reset(TimeSpan.FromSeconds(_holdSeconds));

            return state;
        }

        private IState holdState(ZibithLogoState state)
        {
            state.StateTransition.AddProgress(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (state.StateTransition.IsFinished)
            {
                return initFadeOutState(state);
            }

            return state;
        }

        private IState initFadeOutState(ZibithLogoState state)
        {
            state.StateIndex = 3;
            state.StateTransition.Reset(TimeSpan.FromSeconds(_fadeOutSeconds));

            return state;
        }

        private IState fadeOutState(ZibithLogoState state)
        {
            state.StateTransition.AddProgress(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(1f, 0f, state.StateTransition.Position));
            if (state.StateTransition.IsFinished)
            {
                return new MainMenuScreenState();
            }
            return state;
        }

        public void RenderState()
        {
            _renderService.Render(batch =>
            {
                ZibithLogoState _state = null;
                batch.Draw(_state.BackgroundTexture.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
                batch.Draw(_state.LogoTexture.Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), _state.LogoColor);
            });
        }

        public void Close()
        {
            ZibithLogoState _state = null;
            unloadTexturesFromState(_state);
        }

        public static IState unloadTexturesFromState(ZibithLogoState state)
        {
            state.BackgroundTexture.Unload();
            state.LogoTexture.Unload();

            return state;
        }
    }
}
