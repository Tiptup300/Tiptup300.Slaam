using SlaamMono.Library;

namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequest : IRequest
    {
        public LobbyScreen ParentScreen { get; private set; }

        public BoardSelectionScreenRequest(LobbyScreen parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
