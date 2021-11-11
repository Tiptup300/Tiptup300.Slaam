using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class SurvivalGameScreenResolver : IRequest<GameScreenRequest, SurvivalGameScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;
        private readonly IRequest<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;

        public SurvivalGameScreenResolver(
            ILogger logger,
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphics,
            IRequest<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
        }

        public SurvivalGameScreen Execute(GameScreenRequest request)
        {
            SurvivalGameScreen output;

            output = new SurvivalGameScreen(_logger, _screenDirector, _resources, _graphics, _gameScreenScoreBoardResolver);
            output.Initialize(request);

            return output;
        }
    }
}
