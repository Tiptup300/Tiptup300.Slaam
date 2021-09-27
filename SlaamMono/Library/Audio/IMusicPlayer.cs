namespace SlaamMono.Library.Audio
{
    public interface IMusicPlayer
    {
        void Play(MusicTrack musicTrack);
        void Stop();
    }
}