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

        public SurvivalGameScreenResolver(ILogger logger, IScreenManager screenDirector, IResources resources, IGraphicsState graphics)
        {
            _logger = logger;
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
        }

        public SurvivalGameScreen Execute(GameScreenRequest request)
        {
            SurvivalGameScreen output;

            output = new SurvivalGameScreen(_logger, _screenDirector, _resources, _graphics);
            output.Initialize(request);

            return output;
        }
    }
}
