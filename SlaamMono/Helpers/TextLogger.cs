using System.IO;

namespace SlaamMono
{

    /// <summary>
    /// Class to help with logging events and important information for bugs.
    /// </summary>
    public class TextLogger : ILogger
    {
        public static TextLogger Instance = new TextLogger();

        private TextWriter _textWriter;

        /// <summary>
        /// Prepares the log for writing.
        /// </summary>
        public void Begin()
        {
            _textWriter = File.CreateText("log.log");


            Log("=======================================");
            Log("Slaam! - Logfile (for errors)");
            Log(" Created by Tiptup300");
            Log("=======================================");
            Log("");
        }


        /// <summary>
        /// Writes to log with formatting lines
        /// </summary>
        /// <param name="str">String to be written.</param>
        public void Log(string str)
        {
            _textWriter.WriteLine(str);
        }

        /// <summary>
        /// Closes the log.
        /// </summary>
        public void End()
        {
            Log("Game Closed");

            _textWriter.Close();
        }
    }

}
