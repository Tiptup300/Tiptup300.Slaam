using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Library
{
    public interface ICommand<TInputRequest>
    {
        void Execute(TInputRequest request);
    }
}
