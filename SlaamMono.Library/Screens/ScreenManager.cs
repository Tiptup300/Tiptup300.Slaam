using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public class ScreenManager : IScreenManager
    {
        private readonly Mut<ScreenState> _screenState;
        private readonly IResolver<ScreenRequest, IScreen> _screenResolver;

        public ScreenManager(
            Mut<ScreenState> screenState,
            IResolver<ScreenRequest, IScreen> screenResolver)
        {
            _screenState = screenState;
            _screenResolver = screenResolver;
        }

        private bool _hasCurrentScreen => _screenState.Get().CurrentScreen != null;

        public void Update()
        {
            if (_hasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Update();
            }

            if (_screenState.Get().IsChangingScreens)
            {
                changeScreen();
            }
        }

        private void changeScreen()
        {
            if (_hasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Close();
            }
            _screenState.Mutate(new ScreenState(_screenState.Get().NextScreen, null, false));
            _screenState.Get().CurrentScreen.Open();
            _screenState.Get().CurrentScreen.Update();
        }

        public void Draw(SpriteBatch batch)
        {
            if (_hasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Draw(batch);
            }
        }

        public void ChangeTo(IScreen nextScreen)
        {
            _screenState.Mutate(new ScreenState(_screenState.Get().CurrentScreen, nextScreen, true));
        }

        public void ChangeTo<TScreenType>() where TScreenType : IScreen
        {
            ChangeTo(_screenResolver.Resolve(new ScreenRequest(typeof(TScreenType).Name)));
        }
    }
}
