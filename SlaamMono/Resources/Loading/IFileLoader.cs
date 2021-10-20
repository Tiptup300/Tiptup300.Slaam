using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Resources.Loading
{
    public interface IFileLoader
    {
        object Load(string filePath);
    }
    public interface IFileLoader<T> : IFileLoader
    {
    }
}
