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
            LogHelper.Instance.Begin();

            LogHelper.Write("Program executed;");
            LogHelper.Write("XNA Starting...");

            try
            {
            using (SlaamGame game = new SlaamGame())
            {
                game.Run();
            }

            }
            catch (Exception e)
            {
                LogHelper.Write("Game Failed With Exit Message: ");
                LogHelper.Write(e.Message);
                if (e.InnerException != null)
                {
                    LogHelper.Write(" --- INNER Exception --- ");
                    LogHelper.Write(e.InnerException.ToString());
                }
                LogHelper.Write(" --- STACK TRACE --- ");
                LogHelper.Write(e.StackTrace);
                LogHelper.End();
            }
        }
    }
}

