using SlaamMono.Library.Timing;

namespace SlaamMono.Library
{
    public interface IFrameTimeService
    {
        FrameInfo GetLatestFrame();
    }
}