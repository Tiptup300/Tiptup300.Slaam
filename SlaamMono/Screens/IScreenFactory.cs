namespace SlaamMono.Screens
{
    public interface IScreenFactory
    {
        IScreen Get(string name);
    }
}
