using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlaamMono.Library.Screens
{
    public class ScreenFactory : IScreenFactory
    {
        private Dictionary<string, Type> _screens;
        private readonly IResolver _resolver;

        public ScreenFactory(IResolver resolver)
        {
            _screens = getScreens();
            _resolver = resolver;
        }

        private Dictionary<string, Type> getScreens()
        {
            return Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IScreen)))
                .ToDictionary(t => t.Name);
        }

        public IScreen Get(string screenName)
            => (IScreen)_resolver.x_Get(_screens[screenName]);

        public IScreen GetScreen<TScreenType>()
            => (IScreen)_resolver.x_Get<TScreenType>();
    }
}
