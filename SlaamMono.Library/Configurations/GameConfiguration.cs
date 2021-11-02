namespace SlaamMono.Library.Configurations
{
    public class GameConfiguration
    {
        public GameConfiguration(bool showFPS)
        {
            ShowFPS = showFPS;
        }

        public bool ShowFPS { get; private set; }
    }
}
