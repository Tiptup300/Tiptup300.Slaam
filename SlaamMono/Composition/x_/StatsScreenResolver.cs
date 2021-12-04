using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library;
using SlaamMono.Library.Logging;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.Library.Screens;

namespace SlaamMono.Composition.x_
{
    public class StatsScreenResolver : IResolver<StatsScreenRequest, StatsScreen>
    {
        private readonly ILogger _logger;
        private readonly IScreenManager _screenManager;
        private readonly IResources _resources;
        private readonly IRenderGraph _renderGraph;

        public StatsScreenResolver(
            ILogger logger,
            IScreenManager screenManager,
            IResources resources,
            IRenderGraph renderGraph)
        {
            _logger = logger;
            _screenManager = screenManager;
            _resources = resources;
            _renderGraph = renderGraph;
        }

        public StatsScreen Resolve(StatsScreenRequest request)
        {
            StatsScreen output;

            output = new StatsScreen(_logger, _screenManager, _resources, _renderGraph);
            output.Initialize(request);

            return output;
        }
    }
}
