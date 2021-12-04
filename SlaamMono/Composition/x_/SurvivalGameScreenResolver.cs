using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Survival;

namespace SlaamMono.Composition.x_
{
    public class SurvivalGameScreenResolver : IResolver<GameScreenRequest, SurvivalGameScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;
        private readonly IResolver<StatsScreenRequest, StatsScreen> _statsScreenResolver;

        public SurvivalGameScreenResolver(
            ILogger logger,
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphics,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver,
            IResolver<StatsScreenRequest, StatsScreen> statsScreenResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
            _statsScreenResolver = statsScreenResolver;
        }

        public SurvivalGameScreen Resolve(GameScreenRequest request)
        {
            SurvivalGameScreen output;

            output = new SurvivalGameScreen(_logger, _screenDirector, _resources, _graphics, _gameScreenScoreBoardResolver, _statsScreenResolver);
            output.Initialize(request);

            return output;
        }
    }
}
