using System;

namespace SlaamMono.Library
{
    public interface IResolver
    {
        T Get<T>();
        object Get(Type type);
    }
}
