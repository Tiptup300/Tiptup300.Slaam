namespace SlaamMono
{
    public class GraphicsConfiguration
    {
        public int GameWidth { get; private set; }
        public int GameHeight { get; private set; }
        public bool MultiSampling { get; private set; }
        public bool IsFullScreen { get; private set; }

        public GraphicsConfiguration(int gameWidth, int gameHeight, bool multiSampling, bool isFullScreen)
        {
            GameWidth = gameWidth;
            GameHeight = gameHeight;
            MultiSampling = multiSampling;
            IsFullScreen = isFullScreen;
        }
    }
}
