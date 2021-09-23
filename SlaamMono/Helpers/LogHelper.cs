using System.IO;

namespace SlaamMono
{

    /// <summary>
    /// Class to help with logging events and important information for bugs.
    /// </summary>
    public class LogHelper
    {
        public static LogHelper Instance = new LogHelper();

        private static TextWriter _textWriter;

        /// <summary>
        /// Prepares the log for writing.
        /// </summary>
        public void Begin()
        {
            _textWriter = File.CreateText("log.log");


            Write("=======================================");
            Write("Slaam! - Logfile (for errors)");
            Write(" Created by Tiptup300");
            Write("=======================================");
            Write("");
        }


        /// <summary>
        /// Writes to log with formatting lines
        /// </summary>
        /// <param name="str">String to be written.</param>
        public static void Write(string str)
        {
            _textWriter.WriteLine(str);
        }

        /// <summary>
        /// Closes the log.
        /// </summary>
        public static void End()
        {
            Write("Game Closed");

            _textWriter.Close();
        }
    }

}
