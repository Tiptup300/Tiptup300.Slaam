namespace SlaamMono.Resources.Loading
{
    public interface IFileLoader
    {
        object Load(string filePath);
    }
    public interface IFileLoader<T> : IFileLoader
    {
    }
}
