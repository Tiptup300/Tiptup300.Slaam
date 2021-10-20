using System.Collections.Generic;

namespace SlaamMono.Resources.Loaders
{
    public interface ITextLineLoader
    {
        IEnumerable<string> LoadTextLines(string directoryPath, string baseName);
    }
}