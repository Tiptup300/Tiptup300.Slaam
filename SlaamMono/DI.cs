using SimpleInjector;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    public class DI : IResolver
    {
        public static DI Instance = new DI();

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private Container _container;

        public DI()
        {
            _container = new Bootstrap().BuildContainer();
        }

        public T Get<T>()
        {
            if(_instances.ContainsKey(typeof(T)) == false)
            {
                _instances.Add(typeof(T), _container.GetInstance(typeof(T)));
            }
            return (T)_instances[typeof(T)];
        }
    }
}
