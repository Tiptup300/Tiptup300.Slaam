namespace SlaamMono.Library.Resources
{
    public interface IFileLoader
    {
        object Load(string filePath);
    }
    public interface IFileLoader<T> : IFileLoader
    {
    }
}
