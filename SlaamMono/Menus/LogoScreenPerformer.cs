using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Subclasses;
using SlaamMono.x_;
using System;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus
{
    public class LogoScreenPerformer : ILogoScreen, IStatePerformer
    {
        private const float _fadeInSeconds = 1f;
        private const float _holdSeconds = 3f;
        private const float _fadeOutSeconds = 0.5f;

        private LogoScreenState _state;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IResolver<TextureRequest, CachedTexture> _textureRequest;

        public LogoScreenPerformer(IScreenManager screenDirector, IResources resources, IResolver<TextureRequest, CachedTexture> textureRequest)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _textureRequest = textureRequest;
        }

        public void InitializeState()
        {
            _state = new LogoScreenState();
        }



        public IState Perform()
        {
            switch (_state.StateIndex)
            {
                default: return loadTextures(_state);
                case 0: return initFadeInState(_state);
                case 1: return fadeInState(_state);
                case 2: return holdState(_state);
                case 3: return fadeOutState(_state);
            }
        }

        private IState loadTextures(LogoScreenState state)
        {
            state.BackgroundTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogoBG"));
            state.LogoTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogo"));

            return state;
        }

        private static IState initFadeInState(LogoScreenState state)
        {
            state.StateIndex = 1;
            state.StateTransition = new Transition(TimeSpan.FromSeconds(_fadeInSeconds));
            state.LogoColor = new Color(Color.White, 0);

            return state;
        }

        private static IState fadeInState(LogoScreenState state)
        {
            state.StateTransition.AddProgress(FrameRateDirector.Instance.GetLatestFrame().MovementFactorTimeSpan);
            state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(0f, 1f, state.StateTransition.Position));
            if (state.StateTransition.IsFinished)
            {
                return initHoldState(state);
            }

            return state;
        }

        private static IState initHoldState(LogoScreenState state)
        {
            state.StateIndex = 2;
            state.LogoColor = Color.White;
            state.StateTransition.Reset(TimeSpan.FromSeconds(_holdSeconds));

            return state;
        }

        private static IState holdState(LogoScreenState state)
        {
            state.StateTransition.AddProgress(FrameRateDirector.Instance.GetLatestFrame().MovementFactorTimeSpan);
            if (state.StateTransition.IsFinished)
            {
                return initFadeOutState(state);
            }

            return state;
        }

        private static IState initFadeOutState(LogoScreenState state)
        {
            state.StateIndex = 3;
            state.StateTransition.Reset(TimeSpan.FromSeconds(_fadeOutSeconds));

            return state;
        }

        private static IState fadeOutState(LogoScreenState state)
        {
            state.StateTransition.AddProgress(FrameRateDirector.Instance.GetLatestFrame().MovementFactorTimeSpan);
            state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(1f, 0f, state.StateTransition.Position));
            if (state.StateTransition.IsFinished)
            {
                return new MainMenuScreenState();
            }
            return state;
        }

        public void RenderState(SpriteBatch batch)
        {
            batch.Draw(_state.BackgroundTexture.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(_state.LogoTexture.Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), _state.LogoColor);
        }

        public void Close()
        {
            unloadTexturesFromState(_state);
        }

        public static IState unloadTexturesFromState(LogoScreenState state)
        {
            state.BackgroundTexture.Unload();
            state.LogoTexture.Unload();

            return state;
        }
    }
}
