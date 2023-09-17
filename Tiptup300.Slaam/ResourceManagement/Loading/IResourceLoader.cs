namespace SlaamMono.ResourceManagement.Loading;

 public interface IResourceLoader
 {
     T Load<T>(string resourceName) where T : class;
 }