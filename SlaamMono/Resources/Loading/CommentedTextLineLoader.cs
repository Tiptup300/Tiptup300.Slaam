using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.Resources.Loading
{
    public class CommentedTextLineLoader : IFileLoader<string[]>
    {
        public object Load(string baseName)
        {
            return File.ReadAllLines(baseName)
                .Where(line => line.StartsWith("//") == false)
                .ToArray();
        }
    }
}
