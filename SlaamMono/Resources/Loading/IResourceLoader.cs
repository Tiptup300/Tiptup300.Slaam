namespace SlaamMono.Resources.Loading
{
    public interface IResourceLoader
    {
        T LoadResource<T>(string resourceName) where T : class;
    }
}