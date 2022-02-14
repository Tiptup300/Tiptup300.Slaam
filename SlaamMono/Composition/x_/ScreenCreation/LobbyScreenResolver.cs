using SlaamMono.Gameplay;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.MatchCreation;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_.ScreenCreation
{
    public class LobbyScreenResolver : IResolver<LobbyScreenRequest, LobbyScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly PlayerColorResolver _playerColorResolver;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraphManager;
        private readonly IResolver<GameScreenRequest, GameScreen> _gameScreenRequest;
        private readonly IResolver<BoardSelectionScreenRequest, BoardSelectionScreen> _boardSelectionScreenResolver;
        private readonly IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen> _characterSelectionScreenResolver;

        public LobbyScreenResolver(
            ILogger logger,
            IScreenManager screenDirector,
            PlayerColorResolver playerColorResolver,
            IResources resources,
            IRenderGraph renderGraphManager,
            IResolver<GameScreenRequest, GameScreen> gameScreenRequest,
            IResolver<BoardSelectionScreenRequest, BoardSelectionScreen> boardSelectionScreenResolver,
            IResolver<CharacterSelectionScreenRequest, CharacterSelectionScreen> characterSelectionScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _playerColorResolver = playerColorResolver;
            _resources = resources;
            _renderGraphManager = renderGraphManager;
            _gameScreenRequest = gameScreenRequest;
            _boardSelectionScreenResolver = boardSelectionScreenResolver;
            _characterSelectionScreenResolver = characterSelectionScreenResolver;
        }

        public LobbyScreen Resolve(LobbyScreenRequest request)
        {
            LobbyScreen output;

            output = new LobbyScreen(_logger, _screenDirector, _playerColorResolver, _resources, _renderGraphManager, _gameScreenRequest, _boardSelectionScreenResolver, _characterSelectionScreenResolver);
            output.Initialize(request);

            return output;
        }
    }
}
