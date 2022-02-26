using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using SlaamMono.Survival;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class SurvivalGameScreenResolver : IResolver<GameScreenRequest, SurvivalGameScreen>
    {
        private readonly SurvivalGameScreen _survivalGameScreen;

        public SurvivalGameScreenResolver(
            SurvivalGameScreen survivalGameScreen)
        {
            _survivalGameScreen = survivalGameScreen;
        }

        public SurvivalGameScreen Resolve(GameScreenRequest request)
        {
            SurvivalGameScreen output;

            output = _survivalGameScreen;
            output.Initialize(request);

            return output;
        }
    }
}
