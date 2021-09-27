using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Helpers.Logging
{
    public interface ILoggingDevice
    {
        void Begin();
        void Log(string line);
        void End();
    }
}
