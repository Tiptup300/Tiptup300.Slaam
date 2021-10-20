using System.Collections.Generic;

namespace SlaamMono.Resources.Loading
{
    public interface ITextLineLoader
    {
        IEnumerable<string> LoadTextLines(string filePath);
    }
}