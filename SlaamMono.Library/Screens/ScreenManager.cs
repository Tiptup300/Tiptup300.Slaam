using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Library.Screens
{
    public class ScreenManager : IScreenManager
    {
        private readonly Mut<ScreenTransitionState> _screenState;
        private readonly IResolver<ScreenRequest, IScreen> _screenResolver;

        public ScreenManager(
            Mut<ScreenTransitionState> screenState,
            IResolver<ScreenRequest, IScreen> screenResolver)
        {
            _screenState = screenState;
            _screenResolver = screenResolver;
        }

        public void Update()
        {
            if (_screenState.Get().HasCurrentScreen)
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
            if (_screenState.Get().HasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Close();
            }
            _screenState.Mutate(_screenState.Get().CompleteTransition());
            _screenState.Get().CurrentScreen.Open();
            _screenState.Get().CurrentScreen.Update();
        }

        public void Draw(SpriteBatch batch)
        {
            if (_screenState.Get().HasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Draw(batch);
            }
        }

        public void ChangeTo(IScreen nextScreen)
        {
            var newState = _screenState.Get().BeginTransition(nextScreen);
            _screenState.Mutate(newState);
        }

        public void ChangeTo<TScreenType>() where TScreenType : IScreen
        {
            ChangeTo(_screenResolver.Resolve(new ScreenRequest(typeof(TScreenType).Name)));
        }
    }
}
