using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Slaam
{

    /// <summary>
    /// Class to help with logging events and important information for bugs.
    /// </summary>
    static class LogHelper
    {
        #region Variables

      //  private static StreamWriter LogWriter;

        #endregion

        #region Begin Log

        /// <summary>
        /// Prepares the log for writing.
        /// </summary>
        public static void Begin()
        {
            //LogWriter = File.CreateText("log.log");
        }

        #endregion

        #region Write To Log

        /// <summary>
        /// Writes to log with formatting lines
        /// </summary>
        /// <param name="str">String to be written.</param>
        public static void Write(string str)
        {
            //Write(str, true);
        }

        /// <summary>
        /// Writes to log
        /// </summary>
        /// <param name="str">String to be written.</param>
        /// <param name="lines">Draw Formatting Lines?</param>
        public static void Write(string str, bool lines)
        {
            /*if (!lines)
                LogWriter.WriteLine(str);
            else
                LogWriter.WriteLine("  |  " + str.PadRight(100) + "|");*/
        }

        #endregion

        #region End Log

        /// <summary>
        /// Closes the log.
        /// </summary>
        public static void End()
        {
            //Write("Game Closed");
            //LogWriter.Close();
        }

        #endregion
    }

}
