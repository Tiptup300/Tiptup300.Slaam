using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;

namespace SlaamMono.Composition.x_.ScreenCreation
{
    public class CharacterSelectionScreenResolver : IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResolver<LobbyScreenRequest, LobbyScreen> _lobbyScreenResolver;

        public CharacterSelectionScreenResolver(ILogger logger, IScreenManager screenDirector, IResolver<LobbyScreenRequest, LobbyScreen> lobbyScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _lobbyScreenResolver = lobbyScreenResolver;
        }

        public CharacterSelectionScreen Resolve(CharacterSelectionScreenRequest request)
        {
            CharacterSelectionScreen output;

            output = new CharacterSelectionScreen(_logger, _screenDirector, _lobbyScreenResolver);
            output.Initialize(request);

            return output;
        }
    }
}
