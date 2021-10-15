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

        public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type type)
        {
            if(_instances.ContainsKey(type) == false)
            {
                _instances.Add(type, _container.GetInstance(type));
            }
            return _instances[type];
        }
    }
}
