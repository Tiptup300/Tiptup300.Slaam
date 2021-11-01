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
        private Timer displaytime = new Timer(new TimeSpan(0, 0, 1));

        private Transition LogoColor = new Transition(null, new Vector2(0), new Vector2(255), TimeSpan.FromSeconds(1));
        private bool hasShown = false;

        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;

        public LogoScreen(IScreenManager screenDirector, IResources resources)
        {
            _screenDirector = screenDirector;
            _resources = resources;
        }

        public void Open() { }

        public void Update()
        {
            if (!hasShown)
            {
                LogoColor.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (LogoColor.IsFinished())
                {
                    hasShown = true;
                    displaytime.Reset();
                }
            }
            else
            {
                displaytime.Update(FrameRateDirector.MovementFactorTimeSpan);
                LogoColor.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (displaytime.Active)
                {
                    LogoColor.Reverse(null);
                }
                if (LogoColor.Goal.X == 0 && LogoColor.IsFinished())
                {
                    // if (ProfileManager.FirstTime)
                    //  ScreenHelper.ChangeScreen(new FirstTimeScreen());
                    // else

                    _screenDirector.ChangeTo<IMainMenuScreen>();
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            byte alpha = (byte)LogoColor.Position.X;
            batch.Draw(_resources.GetTexture("ZibithLogoBG").Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(_resources.GetTexture("ZibithLogo").Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - _resources.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - _resources.GetTexture("ZibithLogo").Height / 2), new Color((byte)255, (byte)255, (byte)255, alpha));
        }

        public void Close()
        {
            _resources.GetTexture("ZibithLogo").Dispose();
            _resources.GetTexture("ZibithLogoBG").Dispose();
        }
    }
}
