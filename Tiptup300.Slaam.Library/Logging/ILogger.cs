namespace SlaamMono.Library.Logging
{
    public interface ILogger
    {
        void Initialize();
        void End();
        void Log(string str);
    }
}