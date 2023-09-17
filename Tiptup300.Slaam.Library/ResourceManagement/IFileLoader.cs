namespace SlaamMono.Library.ResourceManagement;

 public interface IFileLoader
 {
     object Load(string filePath);
 }
 public interface IFileLoader<T> : IFileLoader
 {
 }
