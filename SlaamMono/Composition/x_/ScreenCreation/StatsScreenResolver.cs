using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;
using ZzziveGameEngine;

namespace SlaamMono.Composition.x_
{
    public class StatsScreenResolver : IResolver<StatsScreenRequest, StatsScreen>
    {
        private readonly StatsScreen _statsScreen;

        public StatsScreenResolver(StatsScreen statsScreen)
        {
            this._statsScreen = statsScreen;
        }

        public StatsScreen Resolve(StatsScreenRequest request)
        {
            StatsScreen output;

            output = _statsScreen;
            output.Initialize(request);

            return output;
        }
    }
}
