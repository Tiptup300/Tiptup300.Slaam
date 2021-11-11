using SimpleInjector;
using SlaamMono.Library;
using System;
using System.Collections.Generic;

namespace SlaamMono.Composition
{
    public class Di : IResolver
    {
        public static IResolver Instance = new Di();

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private Container _container;

        public Di()
        {
            _container = new Composer().BuildContainer(this);
        }

        public T x_Get<T>() => (T)x_Get(typeof(T));

        public object x_Get(Type type)
        {
            if (_instances.ContainsKey(type) == false)
            {
                _instances.Add(type, _container.GetInstance(type));
            }
            return _instances[type];
        }

        public static T Get<T>() => Instance.x_Get<T>();
    }
}
