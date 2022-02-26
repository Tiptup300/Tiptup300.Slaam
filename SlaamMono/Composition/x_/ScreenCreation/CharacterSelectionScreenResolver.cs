using SlaamMono.Library.Logging;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_.ScreenCreation
{
    public class CharacterSelectionScreenResolver : IResolver<CharacterSelectionScreenRequestState, CharacterSelectionScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResolver<LobbyScreenRequestState, LobbyScreen> _lobbyScreenResolver;

        public CharacterSelectionScreenResolver(ILogger logger, IScreenManager screenDirector, IResolver<LobbyScreenRequestState, LobbyScreen> lobbyScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _lobbyScreenResolver = lobbyScreenResolver;
        }

        public CharacterSelectionScreen Resolve(CharacterSelectionScreenRequestState request)
        {
            CharacterSelectionScreen output;

            output = new CharacterSelectionScreen(_logger, _screenDirector, _lobbyScreenResolver);
            output.Initialize(request);

            return output;
        }
    }
}
