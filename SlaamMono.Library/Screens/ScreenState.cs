namespace SlaamMono.Library.Screens
{
    public class ScreenState
    {
        public IScreen CurrentScreen { get; private set; }
        public IScreen NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public ScreenState(IScreen currentScreen, IScreen nextScreen, bool isChangingScreens)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }
    }
}
