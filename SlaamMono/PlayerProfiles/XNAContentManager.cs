using SlaamMono.Composition;
using SlaamMono.Library.Logging;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.PlayerProfiles
{
    public class XnaContentManager
    {
        public bool NeedsDevice => _needsDevice;
        private bool _needsDevice = true;

        public ILogger _logger;

        public XnaContentManager(ILogger logger)
        {
            _logger = logger;
        }

        public void Update()
        {
            _needsDevice = false;

            ProfileManager.Initialize(_logger, x_Di.Get<IResources>());
            _logger.Log("Profile Manager Created;");

        }
    }
}
