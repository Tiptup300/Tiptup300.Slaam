﻿namespace SlaamMono.Library.Logging
{
    public interface ILogger
    {
        void Begin();
        void End();
        void Log(string str);
    }
}