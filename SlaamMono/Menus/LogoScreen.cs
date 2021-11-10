using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.ResourceManagement;
using SlaamMono.Subclasses;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;

namespace SlaamMono.Menus
{
    public class LogoScreen : ILogoScreen, IScreen
    {
        private Timer _displaytime;
        private Transition _logoColor;
        private bool hasShown = false;
        private CachedTexture _backgroundTexture;
        private CachedTexture _logoTexture;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IRequest<TextureRequest, CachedTexture> _textureRequest;

        public LogoScreen(IScreenManager screenDirector, IResources resources, IRequest<TextureRequest, CachedTexture> textureRequest)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _textureRequest = textureRequest;
        }

        public void Open()
        {
            _logoColor = new Transition(null, new Vector2(0), new Vector2(255), TimeSpan.FromSeconds(1));
            _displaytime = new Timer(new TimeSpan(0, 0, 1));

            _backgroundTexture = _textureRequest.Execute(new TextureRequest("ZibithLogoBG"));
            _logoTexture = _textureRequest.Execute(new TextureRequest("ZibithLogo"));
        }

        public void Update()
        {
            if (!hasShown)
            {
                _logoColor.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_logoColor.IsFinished())
                {
                    hasShown = true;
                    _displaytime.Reset();
                }
            }
            else
            {
                _displaytime.Update(FrameRateDirector.MovementFactorTimeSpan);
                _logoColor.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (_displaytime.Active)
                {
                    _logoColor.Reverse(null);
                }
                if (_logoColor.Goal.X == 0 && _logoColor.IsFinished())
                {
                    _screenDirector.ChangeTo<IMainMenuScreen>();
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            byte alpha = (byte)_logoColor.Position.X;
            batch.Draw(_backgroundTexture.Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(_logoTexture.Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), new Color((byte)255, (byte)255, (byte)255, alpha));
        }

        public void Close()
        {
            _backgroundTexture.Dispose();
            _logoTexture.Dispose();
        }
    }
}
