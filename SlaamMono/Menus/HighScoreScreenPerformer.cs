using Microsoft.Xna.Framework;
using SlaamMono.Library.Input;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.StatsBoards;
using SlaamMono.x_;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus
{
    public class HighScoreScreenPerformer : IPerformer<HighScoreScreenState>
    {
        public const int MAX_HIGHSCORES = 7;

        private HighScoreScreenState _state = new HighScoreScreenState();

        private readonly ILogger _logger;
        private readonly IResources _resources;
        private readonly IRenderService _renderService;
        private readonly IInputService _inputService;
        private readonly IResolver<IRequest, IState> _stateResolver;

        public HighScoreScreenPerformer(
            ILogger logger,
            IResources resources,
            IRenderService renderGraph,
            IInputService inputService,
            IResolver<IRequest, IState> stateResolver)
        {
            _logger = logger;
            _resources = resources;
            _renderService = renderGraph;
            _inputService = inputService;
            _stateResolver = stateResolver;
        }

        public void InitializeState()
        {
            _state._statsboard = new SurvivalStatsBoard(
                null, new Rectangle(10, 68, GameGlobals.DRAWING_GAME_WIDTH - 20, GameGlobals.DRAWING_GAME_WIDTH - 20), new Color(0, 0, 0, 150), MAX_HIGHSCORES, _logger, _resources, _renderService,
                null // this will not cause problems, but it's still ugly.
                );

            _state._statsboard.CalculateStats();
            _state._statsboard.ConstructGraph(25);
        }

        public IState Perform(HighScoreScreenState state)
        {
            if (_inputService.GetPlayers()[0].PressedAction2)
            {
                return _stateResolver.Resolve(new MainMenuRequest());
            }
            return state;
        }

        public void Render(HighScoreScreenState state)
        {
            _renderService.Render(batch =>
            {
                state._statsboard.MainBoard.Draw(batch);
            });
        }
    }
}
