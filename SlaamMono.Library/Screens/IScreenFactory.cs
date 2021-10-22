namespace SlaamMono.Library.Screens
{
    public interface IScreenFactory
    {
        IScreen Get(string name);
    }
}
