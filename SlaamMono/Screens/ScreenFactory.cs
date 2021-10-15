using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SlaamMono.Screens
{
    public class ScreenFactory : IScreenFactory
    {
        private Dictionary<string, Type> _screens;

        public ScreenFactory()
        {
            _screens = getScreens();
        }

        private Dictionary<string, Type> getScreens()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IScreen)))
                .ToDictionary(t => t.Name);
        }

        public IScreen Get(string screenName)
        {
            return (IScreen)DI.Instance.Get(_screens[screenName]);
        }
    }
}
