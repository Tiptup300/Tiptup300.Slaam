using Microsoft.Xna.Framework;
using SlaamMono.Library.Timing;
using System;

namespace SlaamMono.Library
{
    public class FrameTimeService : IFrameTimeService
    {
        private FrameTimeServiceState _state = new FrameTimeServiceState();

        private readonly TimeSpan ONE_SECOND = TimeSpan.FromSeconds(1);

        public Frame GetLatestFrame()
        {
            return _state._latestFrame;
        }

        public void AddUpdate(GameTime gameTime)
        {
            _state._framesUpdated++;
            _state._currentTimer += gameTime.ElapsedGameTime;

            if (_state._currentTimer >= ONE_SECOND)
            {
                _state._currentTimer -= ONE_SECOND;
                _state._framesUpdatedLast = _state._framesUpdated;
                _state._framesDrawnLast = _state._framesDrawn;

                _state._framesDrawn = 0;
                _state._framesUpdated = 0;
            }

            _state._latestFrame = new Frame(
                dateTime: DateTime.UtcNow,
                movementFactor: gameTime.ElapsedGameTime.Milliseconds,
                fDPS: _state._framesDrawnLast,
                fUPS: _state._framesUpdatedLast
            );
        }

        public void AddDraw(GameTime gameTime)
        {
            _state._framesDrawn++;
        }
    }
}
