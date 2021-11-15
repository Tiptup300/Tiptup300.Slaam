namespace SlaamMono.Library.Screens
{
    public class ScreenTransitionState
    {
        public IScreen CurrentScreen { get; private set; }
        public IScreen NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public bool HasCurrentScreen => CurrentScreen != null;

        public ScreenTransitionState(IScreen currentScreen, IScreen nextScreen, bool isChangingScreens = false)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }

        public ScreenTransitionState BeginTransition(IScreen nextScreen)
        {
            return new ScreenTransitionState(CurrentScreen, nextScreen, true);
        }

        public ScreenTransitionState CompleteTransition()
        {
            return new ScreenTransitionState(NextScreen, null, false);
        }
    }
}
