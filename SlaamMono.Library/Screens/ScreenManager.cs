using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public class ScreenManager : IScreenManager
    {
        private ScreenState screenState = new ScreenState(null, null, false);

        private readonly IResolver<ScreenRequest, IScreen> _screenResolver;

        public ScreenManager(IResolver<ScreenRequest, IScreen> screenResolver)
        {
            _screenResolver = screenResolver;
        }

        private bool _hasCurrentScreen => screenState.CurrentScreen != null;

        public void Update()
        {
            if (_hasCurrentScreen)
            {
                screenState.CurrentScreen.Update();
            }

            if (screenState.IsChangingScreens)
            {
                changeScreen();
            }
        }

        private void changeScreen()
        {
            if (_hasCurrentScreen)
            {
                screenState.CurrentScreen.Close();
            }
            screenState = new ScreenState(screenState.NextScreen, null, false);
            screenState.CurrentScreen.Open();
            screenState.CurrentScreen.Update();
        }

        public void Draw(SpriteBatch batch)
        {
            if (_hasCurrentScreen)
            {
                screenState.CurrentScreen.Draw(batch);
            }
        }

        public void ChangeTo(IScreen nextScreen)
        {
            screenState = new ScreenState(screenState.CurrentScreen, nextScreen, true);
        }

        public void ChangeTo<TScreenType>() where TScreenType : IScreen
        {
            ChangeTo(_screenResolver.Resolve(new ScreenRequest(typeof(TScreenType).Name)));
        }
    }
}
