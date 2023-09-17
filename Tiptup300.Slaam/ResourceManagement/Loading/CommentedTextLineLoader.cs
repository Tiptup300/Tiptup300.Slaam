using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.ResourceManagement.Loading;

public class CommentedTextLineLoader : IFileLoader<string[]>
{
   public object Load(string baseName)
   {
      return System.Text.Json.JsonSerializer.Deserialize<string[]>(File.ReadAllText(baseName));
   }
}
