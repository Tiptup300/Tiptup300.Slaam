#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SlaamMono
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FPSManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static float MovementFactor { private set; get; }
        public static TimeSpan MovementFactorTimeSpan { get { return new TimeSpan(0, 0, 0, 0, (int)MovementFactor); } }
        private int framesDrawn, framesUpdated;
        private TimeSpan oneSecond = TimeSpan.FromSeconds(1), currentTimer = TimeSpan.Zero;
        private int framesDrawnLast, framesUpdatedLast;

        public static int FDPS { private set; get; }

        public static int FUPS { private set; get; }

        public FPSManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
            // TODO: Add your update code here
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            framesDrawn++;
            base.Draw(gameTime);
        }
    }
}


