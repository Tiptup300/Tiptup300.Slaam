namespace SlaamMono.Library.Screens
{
    public class ScreenTransitionState
    {
        public IStateUpdater CurrentScreen { get; private set; }
        public IStateUpdater NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public bool HasCurrentScreen => CurrentScreen != null;

        public ScreenTransitionState(IStateUpdater currentScreen, IStateUpdater nextScreen, bool isChangingScreens = false)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }

        public ScreenTransitionState BeginTransition(IStateUpdater nextScreen)
        {
            return new ScreenTransitionState(CurrentScreen, nextScreen, true);
        }

        public ScreenTransitionState CompleteTransition()
        {
            return new ScreenTransitionState(NextScreen, null, false);
        }
    }
}
