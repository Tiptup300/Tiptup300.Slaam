namespace SlaamMono.Library.Logging;

 public interface ILoggingDevice
 {
     void Begin();
     void Log(string line);
     void End();
 }
