using Microsoft.Xna.Framework;
using SlaamMono.Library.Logging;

namespace SlaamMono.Library.Audio
{
    public class AudioManager : GameComponent, IMusicPlayer
    {
        private readonly ILogger _logger;

        public AudioManager(SlaamGame game, ILogger logger)
            : base(game)
        {
            _logger = logger;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void Play(MusicTrack musicTrack)
        {
            _logger.Log($"Attempted to play music track: {musicTrack}");
        }

        public void Stop()
        {
            _logger.Log($"Attempted to stop music track.");
        }
    }
}
