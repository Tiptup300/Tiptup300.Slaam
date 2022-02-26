using SlaamMono.Menus;
using SlaamMono.PlayerProfiles;
using SlaamMono.Survival;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_.ScreenCreation
{
    public class x_ScreensResolver :
        IResolver<FirstTimeScreenRequestState, FirstTimeScreen>,
        IResolver<HighScoreScreenRequestState, HighScoreScreen>,
        IResolver<LogoScreenRequestState, LogoScreen>,
        IResolver<MainMenuRequest, MainMenuScreen>,
        IResolver<ProfileEditScreenRequestState, ProfileEditScreen>,
        IResolver<SurvivalChracterSelectionScreenRequestState, SurvivalCharacterSelectionScreen>,
        IResolver<CreditsScreenRequestState, CreditsScreen>
    {
        private FirstTimeScreen _firstTimeScreen;
        private HighScoreScreen _highScoreScreen;
        private LogoScreen _logoScreen;
        private MainMenuScreen _mainMenuScreen;
        private ProfileEditScreen _profileEditScreen;
        private SurvivalCharacterSelectionScreen _survivalCharacterSelectionScreen;
        private CreditsScreen _creditsScreen;

        public x_ScreensResolver(FirstTimeScreen firstTimeScreen, HighScoreScreen highScoreScreen, LogoScreen logoScreen, MainMenuScreen mainMenuScreen, ProfileEditScreen profileEditScreen, SurvivalCharacterSelectionScreen survivalCharacterSelectionScreen, CreditsScreen creditsScreen)
        {
            _firstTimeScreen = firstTimeScreen;
            _highScoreScreen = highScoreScreen;
            _logoScreen = logoScreen;
            _mainMenuScreen = mainMenuScreen;
            _profileEditScreen = profileEditScreen;
            _survivalCharacterSelectionScreen = survivalCharacterSelectionScreen;
            _creditsScreen = creditsScreen;
        }

        public FirstTimeScreen Resolve(FirstTimeScreenRequestState request)
        {
            return _firstTimeScreen;
        }

        public HighScoreScreen Resolve(HighScoreScreenRequestState request)
        {
            return _highScoreScreen;
        }

        public LogoScreen Resolve(LogoScreenRequestState request)
        {
            return _logoScreen;
        }

        public MainMenuScreen Resolve(MainMenuRequest request)
        {
            return _mainMenuScreen;
        }

        public ProfileEditScreen Resolve(ProfileEditScreenRequestState request)
        {
            return _profileEditScreen;
        }

        public SurvivalCharacterSelectionScreen Resolve(SurvivalChracterSelectionScreenRequestState request)
        {
            return _survivalCharacterSelectionScreen;
        }

        public CreditsScreen Resolve(CreditsScreenRequestState request)
        {
            return _creditsScreen;
        }
    }
}
