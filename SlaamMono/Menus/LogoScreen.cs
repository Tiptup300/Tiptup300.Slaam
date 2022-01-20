using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Subclasses;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;

namespace SlaamMono.Menus
{
    public class LogoScreen : ILogoScreen, IScreen
    {
        private LogoScreenState _state;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IResolver<TextureRequest, CachedTexture> _textureRequest;

        public LogoScreen(IScreenManager screenDirector, IResources resources, IResolver<TextureRequest, CachedTexture> textureRequest)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _textureRequest = textureRequest;
        }

        public void Open()
        {
            initState();
        }

        private void initState()
        {
            _state = new LogoScreenState()
            {
                LogoColorTransition = new Transition(new Vector2(0), new Vector2(255), TimeSpan.FromSeconds(1)),
                DisplayTime = new Timer(new TimeSpan(0, 0, 1)),
                BackgroundTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogoBG")),
                LogoTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogo"))
            };
        }

        public void Update()
        {
            _state.LogoColorTransition.Update(FrameRateDirector.MovementFactorTimeSpan);
            if (!_state.HasShown && _state.LogoColorTransition.IsFinished())
            {
                _state.HasShown = true;
                _state.DisplayTime.Reset();

            }
            else
            {
                _state.DisplayTime.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_state.DisplayTime.Active)
                {
                    _state.LogoColorTransition.Reverse(null);
                }
                if (_state.LogoColorTransition.Goal.X == 0 && _state.LogoColorTransition.IsFinished())
                {
                    _screenDirector.ChangeTo<IMainMenuScreen>();
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            byte alpha = (byte)_state.LogoColorTransition.Position.X;
            batch.Draw(_state.BackgroundTexture.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(_state.LogoTexture.Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), new Color((byte)255, (byte)255, (byte)255, alpha));
        }

        public void Close()
        {
            _state.BackgroundTexture.Dispose();
            _state.LogoTexture.Dispose();
        }
    }
}
