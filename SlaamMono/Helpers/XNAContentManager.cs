namespace SlaamMono
{
    public static class XNAContentManager
    {
        public static bool NeedsDevice = true;

        public static void Initialize()
        {
        }

        public static void Update()
        {
            NeedsDevice = false;

            ProfileManager.Initialize();
            LogHelper.Instance.Write("Profile Manager Created;");

        }
    }
}
