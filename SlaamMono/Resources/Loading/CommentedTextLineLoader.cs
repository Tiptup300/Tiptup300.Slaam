using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.Resources.Loading
{
    public class CommentedTextLineLoader : ITextLineLoader
    {
        public IEnumerable<string> LoadTextLines(string baseName)
        {
            return File.ReadAllLines(baseName)
                .Where(line => line.StartsWith("//") == false);
        }
    }
}
