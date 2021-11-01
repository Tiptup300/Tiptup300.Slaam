using SlaamMono.Library;
using SlaamMono.Library.Logging;
using System;

namespace SlaamMono
{
    static class Program
    {
        public static byte[] Version = new byte[] { 000, 000, 000, 002, };

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Di.Get<IApp>()
                .Run();
        }

    }
}

