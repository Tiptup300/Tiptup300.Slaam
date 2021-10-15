using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Screens
{
    public interface IScreenFactory
    {
        IScreen Get(string name);
    }
}
