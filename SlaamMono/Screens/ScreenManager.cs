using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;

namespace SlaamMono.Screens
{
    public class ScreenManager : IScreenManager
    {
        private IScreen _currentScreen;
        private IScreen _nextScreen;
        private bool _isChangingScreens = false;

        public ScreenManager()
        {
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
            BackgroundManager.ChangeBG(BackgroundManager.BackgroundType.Normal);
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
    }
}
