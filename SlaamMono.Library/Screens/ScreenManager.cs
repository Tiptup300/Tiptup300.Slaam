using Microsoft.Xna.Framework.Graphics;
using ZzziveGameEngine;

namespace SlaamMono.Library.Screens
{
    public class ScreenManager : IScreenManager
    {
        private readonly Mut<ScreenTransitionState> _screenState;
        private readonly IResolver<ScreenRequest, IStatePerformer> _screenResolver;

        public ScreenManager(
            Mut<ScreenTransitionState> screenState,
            IResolver<ScreenRequest, IStatePerformer> screenResolver)
        {
            _screenState = screenState;
            _screenResolver = screenResolver;
        }

        public void Update()
        {
            if (_screenState.Get().HasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.Perform();
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
            _screenState.Get().CurrentScreen.InitializeState();
            _screenState.Get().CurrentScreen.Perform();
        }

        public void Draw(SpriteBatch batch)
        {
            if (_screenState.Get().HasCurrentScreen)
            {
                _screenState.Get().CurrentScreen.RenderState(batch);
            }
        }

        public void ChangeTo(IStatePerformer nextScreen)
        {
            var newState = _screenState.Get().BeginTransition(nextScreen);
            _screenState.Mutate(newState);
        }

        public void ChangeTo<TScreenType>() where TScreenType : IStatePerformer
        {
            ChangeTo(_screenResolver.Resolve(new ScreenRequest(typeof(TScreenType).Name)));
        }
    }
}
