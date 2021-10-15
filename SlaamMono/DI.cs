using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    public class DI : IResolver
    {
        public static DI Instance = new DI();

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();


        public DI()
        {

        }

        public T Get<T>()
        {
            return (T)_instances[typeof(T)];
        }

        public void Set<T>(T obj)
        {
            _instances.Add(obj.GetType(), obj);
        }
    }
}
