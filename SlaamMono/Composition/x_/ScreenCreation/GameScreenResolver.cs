using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class GameScreenResolver : IResolver<GameScreenRequest, GameScreen>
    {
        private readonly IScreenManager _screenDirector;
        private readonly IResources _resources;
        private readonly IGraphicsState _graphics;
        private readonly IResolver<ScoreboardRequest, Scoreboard> _gameScreenScoreBoardResolver;
        private readonly IResolver<StatsScreenRequest, StatsScreen> _statsScreenResolver;

        public GameScreenResolver(
            IScreenManager screenDirector,
            IResources resources,
            IGraphicsState graphics,
            IResolver<ScoreboardRequest, Scoreboard> gameScreenScoreBoardResolver,
            IResolver<StatsScreenRequest, StatsScreen> statsScreenResolver)
        {
            _screenDirector = screenDirector;
            _resources = resources;
            _graphics = graphics;
            _gameScreenScoreBoardResolver = gameScreenScoreBoardResolver;
            _statsScreenResolver = statsScreenResolver;
        }

        public GameScreen Resolve(GameScreenRequest request)
        {
            GameScreen output;

            output = new GameScreen(_screenDirector, _resources, _graphics, _gameScreenScoreBoardResolver, _statsScreenResolver);
            output.Initialize(request);

            return output;
        }
    }
}
