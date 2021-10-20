using Microsoft.Xna.Framework;
using System;


namespace SlaamMono.Helpers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FrameRateDirector : DrawableGameComponent
    {
        public static float MovementFactor { private set; get; }
        public static TimeSpan MovementFactorTimeSpan { get { return new TimeSpan(0, 0, 0, 0, (int)MovementFactor); } }
        private int framesDrawn, framesUpdated;
        private TimeSpan oneSecond = TimeSpan.FromSeconds(1), currentTimer = TimeSpan.Zero;
        private int framesDrawnLast, framesUpdatedLast;

        public static int FDPS { private set; get; }

        public static int FUPS { private set; get; }

        public FrameRateDirector(Game game)
            : base(game)
        {

        }

        public override void Update(GameTime gameTime)
        {
            framesUpdated++;
            currentTimer += gameTime.ElapsedGameTime;

            if (currentTimer >= oneSecond)
            {
                currentTimer -= oneSecond;
                framesUpdatedLast = framesUpdated;
                framesDrawnLast = framesDrawn;

                framesDrawn = 0;
                framesUpdated = 0;
            }

            MovementFactor = gameTime.ElapsedGameTime.Milliseconds;

            FDPS = framesDrawnLast;
            FUPS = framesUpdatedLast;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            framesDrawn++;
            base.Draw(gameTime);
        }
    }
}


