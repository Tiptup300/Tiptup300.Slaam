using SlaamMono.Helpers.Logging;
using System;
using System.IO;

namespace SlaamMono
{

    /// <summary>
    /// Class to help with logging events and important information for bugs.
    /// </summary>
    public class Logger : ILogger
    {
        private ILoggingDevice _loggingDevice;

        public Logger(ILoggingDevice loggingDevice)
        {
            _loggingDevice = loggingDevice;
        }

        public void Begin()
        {
            _loggingDevice.Begin();
            Log("Log Started.");
        }

        public void Log(string line)
        {
            _loggingDevice.Log(line);
            writeLineToConsle(line);
        }

        private void writeLineToConsle(string line) 
            => System.Console.WriteLine(line);

        public void End()
        {
            Log("Game Closed");
            _loggingDevice.End();
        }
    }

}
