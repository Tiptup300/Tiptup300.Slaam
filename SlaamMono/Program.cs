using SlaamMono.Library.Logging;
using System;

namespace SlaamMono
{
    static class Program
    {
        public static byte[] Version = new byte[] { 000, 000, 000, 002, };

        private static ILogger _logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            _logger = new Logger(new TextFileLog());
            DI.Instance.Set(_logger);
            startLog();

            try
            {
                using (SlaamGame game = new SlaamGame(DI.Instance.Get<ILogger>()))
                {
                    game.Run();
                }

            }
            catch (Exception e)
            {
                logError(e);
                throw e;
            }

            _logger.End();
        }

        private static void logError(Exception e)
        {
            _logger.Log("Game Failed With Exit Message: ");
            _logger.Log(e.Message);
            if (e.InnerException != null)
            {
                _logger.Log(" --- INNER Exception --- ");
                _logger.Log(e.InnerException.ToString());
            }
            _logger.Log(" --- STACK TRACE --- ");
            _logger.Log(e.StackTrace);
        }

        private static void startLog()
        {
            _logger.Begin();
            _logger.Log("=======================================");
            _logger.Log("Slaam! - Logfile (for errors)");
            _logger.Log(" Created by Tiptup300");
            _logger.Log("=======================================");
            _logger.Log("");
            _logger.Log("Program executed;");
            _logger.Log("XNA Starting...");
        }
    }
}

