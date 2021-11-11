using SlaamMono.Gameplay;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class GameScreenResolver : IResolver<GameScreenRequest, GameScreen>
    {
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;

        public GameScreenResolver(
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphics,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
        }

        public GameScreen Resolve(GameScreenRequest request)
        {
            GameScreen output;

            output = new GameScreen(_screenDirector, _resources, _graphics, _gameScreenScoreBoardResolver);
            output.Initialize(request);

            return output;
        }
    }
}
