namespace SlaamMono.Library.Screens
{
    public class ScreenTransitionState
    {
        public IStatePerformer CurrentScreen { get; private set; }
        public IStatePerformer NextScreen { get; private set; }
        public bool IsChangingScreens { get; private set; }

        public bool HasCurrentScreen => CurrentScreen != null;

        public ScreenTransitionState(IStatePerformer currentScreen, IStatePerformer nextScreen, bool isChangingScreens = false)
        {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            IsChangingScreens = isChangingScreens;
        }

        public ScreenTransitionState BeginTransition(IStatePerformer nextScreen)
        {
            return new ScreenTransitionState(CurrentScreen, nextScreen, true);
        }

        public ScreenTransitionState CompleteTransition()
        {
            return new ScreenTransitionState(NextScreen, null, false);
        }
    }
}
