using SimpleInjector;
using SlaamMono.Library;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    public class DiImplementer : IResolver
    {
        public static IResolver Instance = new DiImplementer();

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private Container _container;

        public DiImplementer()
        {
            _container = new Bootstrap().BuildContainer(this);
        }

        public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type type)
        {
            if (_instances.ContainsKey(type) == false)
            {
                _instances.Add(type, _container.GetInstance(type));
            }
            return _instances[type];
        }
    }
}
