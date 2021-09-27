using System;

namespace SlaamMono
{
    static class Program
    {
        public static byte[] Version = new byte[] { 000, 000, 000, 002,};

        private static ILogger _logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            _logger = new TextLogger();

            _logger.Begin();
            _logger.Log("Program executed;");
            _logger.Log("XNA Starting...");

            try
            {
            using (SlaamGame game = new SlaamGame(_logger))
            {
                game.Run();
            }

            }
            catch (Exception e)
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

            _logger.End();
        }
    }
}

