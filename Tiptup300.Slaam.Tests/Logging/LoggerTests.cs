using Tiptup300.Slaam.Library.Logging;

namespace Tiptup300.Slaam.Tests.Logging;

public class LoggerTests
{
   [Fact]
   public void DoesBeginWork()
   {
      TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
      Logger logger = new Logger(testLoggingDevice);

      logger.Initialize();
      Assert.True(testLoggingDevice.__Begin__Ran);
   }

   [Fact]
   public void DoesLogWork()
   {
      string expected = "Hello There";
      TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
      Logger logger = new Logger(testLoggingDevice);

      logger.Log("Hello There");
      Assert.True(testLoggingDevice.__Log_Ran);
      Assert.Equal(expected, testLoggingDevice.__Log__Param__line);
   }

   [Fact]
   public void DoesEndWork()
   {

      TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
      Logger logger = new Logger(testLoggingDevice);

      logger.End();
      Assert.True(testLoggingDevice.__End__Ran);
   }
}
