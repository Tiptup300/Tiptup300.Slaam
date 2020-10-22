using System;

namespace Slaam
{
    static class Program
    {
        #region Version

        /// <summary>
        /// Current version of Slaam! Running (for online purposes)
        /// </summary>
        public static byte[] Version = new byte[]
        {
                000,
                000,
                000,
                002,
        };

        #endregion

        #region Entry Point

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            LogHelper.Begin();
            LogHelper.Write("=======================================");
            LogHelper.Write("Slaam! - Logfile (for errors)");
            LogHelper.Write(" Created by Tiptup300");
            LogHelper.Write("=======================================");
            LogHelper.Write("");
            LogHelper.Write("Program executed;");
            LogHelper.Write("XNA Starting...");
#if !DEBUG
            //try
            //{
#endif
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
#if !DEBUG
            /*}
            catch (Exception e)
            {
                LogHelper.Write("Game Failed With Exit Message: ");
                LogHelper.Write(e.Message,false);
                if (e.InnerException != null)
                {
                    LogHelper.Write(" --- INNER Exception --- ", false);
                    LogHelper.Write(e.InnerException.ToString(), false);
                }
                LogHelper.Write(" --- STACK TRACE --- ", false);
                LogHelper.Write(e.StackTrace, false);
                LogHelper.End();
                MessageBox.Show("An error occured, please report this problem, including your log.log \n" +
                                "MSG: \"" + e.Message + "\""
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }*/
#endif
            }

        #endregion
    }
}

