using ZzziveGameEngine.StateManagement;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequestState : IState
    {
        public LobbyScreenPerformer ParentScreen { get; private set; }

        public BoardSelectionScreenRequestState(LobbyScreenPerformer parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
