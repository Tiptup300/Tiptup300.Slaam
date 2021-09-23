using System;

namespace SlaamMono
{
    static class Program
    {
        /// <summary>
        /// Current version of Slaam! Running (for online purposes)
        /// </summary>
        public static byte[] Version = new byte[] { 000, 000, 000, 002,};

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            TextLogger.Instance.Begin();

            TextLogger.Instance.Log("Program executed;");
            TextLogger.Instance.Log("XNA Starting...");

            try
            {
            using (SlaamGame game = new SlaamGame())
            {
                game.Run();
            }

            }
            catch (Exception e)
            {
                TextLogger.Instance.Log("Game Failed With Exit Message: ");
                TextLogger.Instance.Log(e.Message);
                if (e.InnerException != null)
                {
                    TextLogger.Instance.Log(" --- INNER Exception --- ");
                    TextLogger.Instance.Log(e.InnerException.ToString());
                }
                TextLogger.Instance.Log(" --- STACK TRACE --- ");
                TextLogger.Instance.Log(e.StackTrace);
            }

            TextLogger.Instance.End();
        }
    }
}

