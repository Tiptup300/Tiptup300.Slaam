namespace SlaamMono.Resources.Loading
{
    public interface IResourceLoader
    {
        T Load<T>(string resourceName) where T : class;
    }
}