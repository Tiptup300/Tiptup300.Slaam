using ZzziveGameEngine;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequest : IRequest
    {
        public LobbyScreenState ParentScreen { get; private set; }

        public BoardSelectionScreenRequest(LobbyScreenState parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
