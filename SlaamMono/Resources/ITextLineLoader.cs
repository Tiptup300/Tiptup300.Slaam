using System.Collections.Generic;

namespace SlaamMono.Resources
{
    public interface ITextLineLoader
    {
        IEnumerable<string> LoadTextLines(string directoryPath, string baseName);
    }
}