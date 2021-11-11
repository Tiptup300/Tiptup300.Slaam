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
                .Where(t => t.GetInterfaces().Contains(typeof(IScreen)))
                .ToDictionary(t => t.Name);
        }
    }
}
