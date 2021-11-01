using System;

namespace SlaamMono.Library
{
    public interface IResolver
    {
        T x_Get<T>();
        object x_Get(Type type);
    }
}
