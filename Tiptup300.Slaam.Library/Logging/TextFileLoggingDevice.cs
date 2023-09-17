using System.IO;

namespace Tiptup300.Slaam.Library.Logging;

public class TextFileLoggingDevice : ILoggingDevice
{
   private const string DefaultFileName = "log.log";

   private TextWriter _textWriter;
   public void Begin()
   {
      _textWriter = File.CreateText(DefaultFileName);
   }
   public void Log(string line)
   {
      _textWriter.WriteLine(line);
   }

   public void End()
   {
      _textWriter.Close();
   }
}
