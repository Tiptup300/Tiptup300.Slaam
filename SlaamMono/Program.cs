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
            DI.Instance.Get<IApp>()
                .Run();
        }

    }
}

