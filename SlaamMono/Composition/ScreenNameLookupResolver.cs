using SlaamMono.Library;
using SlaamMono.Library.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlaamMono.Composition
{
    public class ScreenNameLookupResolver : IResolver<ScreenNameLookup>
    {
        public ScreenNameLookup Resolve()
        {
            ScreenNameLookup output;

            output = new ScreenNameLookup(getScreenNames());

            return output;
        }

        private Dictionary<string, Type> getScreenNames()
        {
            return typeof(SlaamGameApp).Assembly
                .GetTypes()
                .Where(isIScreen())
                .Where(isMoreThanJustIScreen())
                .ToDictionary(
                    t => getNonIScreenInterface(t).Name,
                    t => getNonIScreenInterface(t)
                );
        }

        private static Func<Type, bool> isIScreen()
        {
            return t => t.GetInterfaces().Contains(typeof(ILogic));
        }

        private static Func<Type, bool> isMoreThanJustIScreen()
        {
            return t => t.GetInterfaces().Count() > 1;
        }

        private static Type getNonIScreenInterface(Type t)
        {
            return t.GetInterfaces().Where(ifce => ifce != typeof(ILogic)).Single();
        }
    }
}
