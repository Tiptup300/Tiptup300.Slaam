using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequestState : IState
    {
        public LobbyScreen ParentScreen { get; private set; }

        public BoardSelectionScreenRequestState(LobbyScreen parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
