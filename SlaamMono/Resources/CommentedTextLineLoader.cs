using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlaamMono.Resources
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
