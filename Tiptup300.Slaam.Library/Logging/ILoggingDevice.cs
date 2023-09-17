namespace Tiptup300.Slaam.Library.Logging;

public interface ILoggingDevice
{
   void Begin();
   void Log(string line);
   void End();
}
