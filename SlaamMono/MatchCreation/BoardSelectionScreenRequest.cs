namespace SlaamMono.MatchCreation
{
    public class BoardSelectionScreenRequest
    {
        public LobbyScreen ParentScreen { get; private set; }

        public BoardSelectionScreenRequest(LobbyScreen parentScreen)
        {
            ParentScreen = parentScreen;
        }
    }
}
