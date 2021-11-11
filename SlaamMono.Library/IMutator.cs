using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Library
{
    public interface IMutator<TState>
    {
        Mut<TState> Resolve();
    }
}
