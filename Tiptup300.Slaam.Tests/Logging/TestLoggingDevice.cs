using SlaamMono.Library.Logging;

namespace SlaamMono.Testing.Logging;

 public class TestLoggingDevice : ILoggingDevice
 {
     public bool __Begin__Ran { get; private set; }
     public void Begin()
     {
         __Begin__Ran = true;
     }

     public bool __End__Ran { get; private set; }
     public void End()
     {
         __End__Ran = true;
     }

     public bool __Log_Ran { get; private set; }
     public string __Log__Param__line { get; private set; }
     public void Log(string line)
     {
         __Log_Ran = true;
         __Log__Param__line = line;
     }
 }
