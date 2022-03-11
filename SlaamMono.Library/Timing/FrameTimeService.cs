using Microsoft.Xna.Framework;
using SlaamMono.Library.Timing;
using System;

namespace SlaamMono.Library
{
    public class FrameTimeService : IFrameTimeService
    {
        public static FrameTimeService Instance { get; private set; }

        private Frame _latestFrame;

        public Frame GetLatestFrame() => _latestFrame;

        private int _framesDrawn;
        private int _framesDrawnLast;
        private int _framesUpdated;
        private int _framesUpdatedLast;

        private readonly TimeSpan _oneSecond = TimeSpan.FromSeconds(1);
        private TimeSpan _currentTimer = TimeSpan.Zero;

        public void Update(GameTime gameTime)
        {
            _framesUpdated++;
            _currentTimer += gameTime.ElapsedGameTime;

            if (_currentTimer >= _oneSecond)
            {
                _currentTimer -= _oneSecond;
                _framesUpdatedLast = _framesUpdated;
                _framesDrawnLast = _framesDrawn;

                _framesDrawn = 0;
                _framesUpdated = 0;
            }

            _latestFrame = new Frame(
                dateTime: DateTime.UtcNow,
                movementFactor: gameTime.ElapsedGameTime.Milliseconds,
                fDPS: _framesDrawnLast,
                fUPS: _framesUpdatedLast
            );
        }

        public void Draw(GameTime gameTime)
        {
            _framesDrawn++;
        }
    }
}
