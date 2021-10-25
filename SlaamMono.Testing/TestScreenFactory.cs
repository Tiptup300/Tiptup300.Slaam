using SlaamMono.Library.Screens;
using System;

namespace SlaamMono.Testing
{
    public class TestScreenFactory : IScreenFactory
    {
        public bool __Get__Ran { get; private set; }
        public string __Get__param__name { get; private set; }
        public IScreen __Get__output { get; set; }
        public IScreen Get(string name)
        {
            __Get__Ran = true;
            __Get__param__name = name;
            return __Get__output;
        }

        public bool __GetScreen__Ran { get; private set; }
        public Type __GetScreen__param__TScreenType { get; private set; }
        public IScreen __GetScreen__output { get; set; }
        public IScreen GetScreen<TScreenType>()
        {
            __GetScreen__Ran = true;
            __GetScreen__param__TScreenType = typeof(TScreenType);
            return __GetScreen__output;
        }
    }
}
