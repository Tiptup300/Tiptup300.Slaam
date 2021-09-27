namespace SlaamMono
{
    public interface ILogger
    {
        void Begin();
        void End();
        void Log(string str);
    }
}