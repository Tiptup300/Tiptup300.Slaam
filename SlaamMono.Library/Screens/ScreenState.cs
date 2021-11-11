namespace SlaamMono.Library.Screens
{
    public class ScreenState
    {
        public IScreen CurrentScreen { get; private set; }
        public IScreen NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public bool HasCurrentScreen => CurrentScreen != null;

        public ScreenState(IScreen currentScreen, IScreen nextScreen, bool isChangingScreens = false)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }

        public ScreenState BeginTransition(IScreen nextScreen)
        {
            return new ScreenState(CurrentScreen, nextScreen, true);
        }

        public ScreenState CompleteTransition()
        {
            return new ScreenState(NextScreen, null, false);
        }
    }
}
