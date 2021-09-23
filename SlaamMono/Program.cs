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

            LogHelper.Instance.Write("Program executed;");
            LogHelper.Instance.Write("XNA Starting...");

            try
            {
            using (SlaamGame game = new SlaamGame())
            {
                game.Run();
            }

            }
            catch (Exception e)
            {
                LogHelper.Instance.Write("Game Failed With Exit Message: ");
                LogHelper.Instance.Write(e.Message);
                if (e.InnerException != null)
                {
                    LogHelper.Instance.Write(" --- INNER Exception --- ");
                    LogHelper.Instance.Write(e.InnerException.ToString());
                }
                LogHelper.Instance.Write(" --- STACK TRACE --- ");
                LogHelper.Instance.Write(e.StackTrace);
            }

            LogHelper.Instance.End();
        }
    }
}

