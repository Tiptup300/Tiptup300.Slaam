using NUnit.Framework;
using SlaamMono.Library.Logging;

namespace SlaamMono.Testing
{
    public class LoggerTests
    {
        [Test]
        public void DoesBeginWork()
        {
            TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
            Logger logger = new Logger(testLoggingDevice);

            logger.Begin();
            Assert.True(testLoggingDevice.__Begin__Ran);
        }

        [Test]
        public void DoesLogWork()
        {
            string expected = "Hello There";
            TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
            Logger logger = new Logger(testLoggingDevice);

            logger.Log("Hello There");
            Assert.True(testLoggingDevice.__Log_Ran);
            Assert.AreEqual(expected, testLoggingDevice.__Log__Param__line);
        }

        [Test]
        public void DoesEndWork()
        {

            TestLoggingDevice testLoggingDevice = new TestLoggingDevice();
            Logger logger = new Logger(testLoggingDevice);

            logger.End();
            Assert.True(testLoggingDevice.__End__Ran);
        }
    }
}
