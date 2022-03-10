using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequestState : IState
    {
        public LobbyScreenState ParentScreen { get; private set; }

        public BoardSelectionScreenRequestState(LobbyScreenState parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
