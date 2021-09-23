namespace SlaamMono
{
    public static class XNAContentManager
    {
        public static bool NeedsDevice = true;

        // todo
        public readonly static ILogger _logger = TextLogger.Instance;

        public static void Initialize()
        {

        }

        public static void Update()
        {
            NeedsDevice = false;

            ProfileManager.Initialize();
            _logger.Log("Profile Manager Created;");

        }
    }
}
