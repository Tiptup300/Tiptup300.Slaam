using System;
using System.Collections.Generic;

namespace SlaamMono.Composition
{
    public class ScreenNameLookup
    {
        public Dictionary<string, Type> ScreenNames { get; private set; }

        public ScreenNameLookup(Dictionary<string, Type> screenNames)
        {
            ScreenNames = screenNames;
        }
    }
}
