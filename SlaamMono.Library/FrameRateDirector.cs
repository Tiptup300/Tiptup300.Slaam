using Microsoft.Xna.Framework;
using System;

namespace SlaamMono.Library
{
    public class FrameRateDirector : DrawableGameComponent
    {
        public static float MovementFactor { private set; get; }
        public static TimeSpan MovementFactorTimeSpan { get { return new TimeSpan(0, 0, 0, 0, (int)MovementFactor); } }

        private int _framesDrawn;
        private int _framesDrawnLast;
        private int _framesUpdated;
        private int _framesUpdatedLast;

        private readonly TimeSpan _oneSecond = TimeSpan.FromSeconds(1);
        private TimeSpan _currentTimer = TimeSpan.Zero;

        public static int FDPS { private set; get; }

        public static int FUPS { private set; get; }

        public FrameRateDirector(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
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

            MovementFactor = gameTime.ElapsedGameTime.Milliseconds;

            FDPS = _framesDrawnLast;
            FUPS = _framesUpdatedLast;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _framesDrawn++;
            base.Draw(gameTime);
        }
    }
}
