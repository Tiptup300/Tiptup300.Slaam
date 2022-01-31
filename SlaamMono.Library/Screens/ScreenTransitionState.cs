namespace SlaamMono.Library.Screens
{
    public class ScreenTransitionState
    {
        public ILogic CurrentScreen { get; private set; }
        public ILogic NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public bool HasCurrentScreen => CurrentScreen != null;

        public ScreenTransitionState(ILogic currentScreen, ILogic nextScreen, bool isChangingScreens = false)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }

        public ScreenTransitionState BeginTransition(ILogic nextScreen)
        {
            return new ScreenTransitionState(CurrentScreen, nextScreen, true);
        }

        public ScreenTransitionState CompleteTransition()
        {
            return new ScreenTransitionState(NextScreen, null, false);
        }
    }
}
