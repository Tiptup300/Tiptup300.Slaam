namespace Tiptup300.Slaam.Library.Logging;

public interface ILogger
{
   void Initialize();
   void End();
   void Log(string str);
}