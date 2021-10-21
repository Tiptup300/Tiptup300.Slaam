using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Resources;
using SlaamMono.Subclasses;
using SlaamMono.SubClasses;
using System;

namespace SlaamMono.Screens
{
    public class LogoScreen : IScreen
    {
        private Timer displaytime = new Timer(new TimeSpan(0, 0, 4));

        private Transition LogoColor = new Transition(null, new Vector2(0), new Vector2(255), TimeSpan.FromSeconds(1));
        private bool hasShown = false;
        private readonly MainMenuScreen _menuScreen;
        private readonly IScreenDirector _screenDirector;

        public LogoScreen(MainMenuScreen menuScreen, IScreenDirector screenDirector)
        {
            _menuScreen = menuScreen;
            _screenDirector = screenDirector;
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
                    _screenDirector.ChangeTo(_menuScreen);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            byte alpha = (byte)LogoColor.Position.X;
            batch.Draw(ResourceManager.Instance.GetTexture("ZibithLogoBG").Texture, new Rectangle(0, 0, GameGlobals.DRAWING_GAME_WIDTH, GameGlobals.DRAWING_GAME_HEIGHT), Color.White);
            batch.Draw(ResourceManager.Instance.GetTexture("ZibithLogo").Texture, new Vector2(GameGlobals.DRAWING_GAME_WIDTH / 2 - ResourceManager.Instance.GetTexture("ZibithLogo").Width / 2, GameGlobals.DRAWING_GAME_HEIGHT / 2 - ResourceManager.Instance.GetTexture("ZibithLogo").Height / 2), new Color((byte)255, (byte)255, (byte)255, alpha));
        }

        public void Close()
        {
            ResourceManager.Instance.GetTexture("ZibithLogo").Dispose();
            ResourceManager.Instance.GetTexture("ZibithLogoBG").Dispose();
        }
    }
}
