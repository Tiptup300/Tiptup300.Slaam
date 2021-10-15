using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    public class InstanceManager : IResolver
    {
        public static InstanceManager Instance = new InstanceManager();

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();


        public InstanceManager()
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
