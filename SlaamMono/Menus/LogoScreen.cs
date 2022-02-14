using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Subclasses;
using SlaamMono.x_;
using System;
using ZzziveGameEngine;

namespace SlaamMono.Menus
{
    public class LogoScreen : ILogoScreen, ILogic
    {
        private LogoScreenState _state;

        private const float _fadeInSeconds = 1f;
        private const float _holdSeconds = 3f;
        private const float _fadeOutSeconds = 0.5f;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IResolver<TextureRequest, CachedTexture> _textureRequest;

        public LogoScreen(IScreenManager screenDirector, IResources resources, IResolver<TextureRequest, CachedTexture> textureRequest)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _textureRequest = textureRequest;
        }

        public void InitializeState()
        {
            _state = new LogoScreenState();
            loadTextures();
        }

        private void loadTextures()
        {
            _state.BackgroundTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogoBG"));
            _state.LogoTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogo"));
        }

        public void UpdateState()
        {
            switch (_state.StateIndex)
            {
                case 0:
                    initFadeInState();
                    break;
                case 1:
                    fadeInState();
                    break;
                case 2:
                    holdState();
                    break;
                case 3:
                    fadeOutState();
                    break;
            }
        }

        private void initFadeInState()
        {
            _state.StateIndex = 1;
            _state.StateTransition = new Transition(TimeSpan.FromSeconds(_fadeInSeconds));
            _state.LogoColor = new Color(Color.White, 0);
        }

        private void fadeInState()
        {
            _state.StateTransition.AddProgress(FrameRateDirector.MovementFactorTimeSpan);
            _state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(0f, 1f, _state.StateTransition.Position));
            if (_state.StateTransition.IsFinished)
            {
                initHoldState();
            }
        }

        private void initHoldState()
        {
            _state.StateIndex = 2;
            _state.LogoColor = Color.White;
            _state.StateTransition.Reset(TimeSpan.FromSeconds(_holdSeconds));
        }

        private void holdState()
        {
            _state.StateTransition.AddProgress(FrameRateDirector.MovementFactorTimeSpan);
            if (_state.StateTransition.IsFinished)
            {
                initFadeOutState();
            }
        }

        private void initFadeOutState()
        {
            _state.StateIndex = 3;
            _state.StateTransition.Reset(TimeSpan.FromSeconds(_fadeOutSeconds));
        }

        private void fadeOutState()
        {
            _state.StateTransition.AddProgress(FrameRateDirector.MovementFactorTimeSpan);
            _state.LogoColor = new Color(Color.White, MathHelper.SmoothStep(1f, 0f, _state.StateTransition.Position));
            if (_state.StateTransition.IsFinished)
            {
                _screenDirector.ChangeTo<IMainMenuScreen>();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_state.BackgroundTexture.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(_state.LogoTexture.Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), _state.LogoColor);
        }

        public void Close()
        {
            _state.BackgroundTexture.Unload();
            _state.LogoTexture.Unload();
        }
    }
}
