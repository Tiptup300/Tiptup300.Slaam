using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.Resources.Loaders
{
    public class CommentedTextLineLoader : ITextLineLoader
    {
        public IEnumerable<string> LoadTextLines(string directoryPath, string baseName)
        {
            return File.ReadAllLines(Path.Combine(directoryPath, $"{baseName}.txt"))
                .Where(line => line.StartsWith("//") == false);
        }
    }
}
