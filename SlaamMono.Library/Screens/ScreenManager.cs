using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public class ScreenManager : IScreenManager
    {
        private IScreen _currentScreen;
        private IScreen _nextScreen;
        private bool _isChangingScreens = false;
        private readonly IResolver<ScreenRequest, IScreen> _screenResolver;

        public ScreenManager(IResolver<ScreenRequest, IScreen> screenResolver)
        {
            _screenResolver = screenResolver;
        }

        private bool _hasCurrentScreen => _currentScreen != null;

        public void Update()
        {
            if (_hasCurrentScreen)
            {
                _currentScreen.Update();
            }

            if (_isChangingScreens)
            {
                changeScreen();
            }
        }

        private void changeScreen()
        {
            if (_hasCurrentScreen)
            {
                _currentScreen.Close();
            }
            _currentScreen = _nextScreen;
            _nextScreen = null;
            _currentScreen.Open();
            _currentScreen.Update();

            _isChangingScreens = false;
        }

        public void Draw(SpriteBatch batch)
        {
            if (_hasCurrentScreen)
            {
                _currentScreen.Draw(batch);
            }
        }

        public void ChangeTo(IScreen nextScreen)
        {
            _isChangingScreens = true;
            _nextScreen = nextScreen;
        }

        public void ChangeTo<TScreenType>() where TScreenType : IScreen
        {
            ChangeTo(_screenResolver.Execute(new ScreenRequest(nameof(TScreenType))));
        }
    }
}
