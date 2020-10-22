#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using FarseerGames.FarseerPhysics;
#endregion



    public class Transition
    {
        public Texture2D TransitionTexture;
        public Vector2 Position;
        public Vector2 Goal;
        public Vector2 StartingValue;
        public TimeSpan Length;
        public TimeSpan Elapsed;

        public Transition(Texture2D texture, Vector2 startingValue, Vector2 goal, TimeSpan length)
        {
            if (texture != null)
                TransitionTexture = texture;
            Position = startingValue;
            StartingValue = startingValue;
            Goal = goal;
            Length = length;
        }

        public bool IsFinished()
        {
            if (Math.Round(Position.X) == Math.Round(Goal.X) &&
                Math.Round(Position.Y) == Math.Round(Goal.Y))
                return true;
            else
                return false;
        }

        public void Update(TimeSpan elapsed)
        {
            Elapsed += elapsed;
            double amount = Elapsed.TotalSeconds / this.Length.TotalSeconds;
            Position = Vector2.SmoothStep(StartingValue, Goal, (float)amount);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(TransitionTexture, Position, Color.White);
        }

        public void Reset()
        {
            Elapsed = TimeSpan.Zero;
        }

        public void Reverse(TimeSpan? timespan)
        {
            Vector2 oldGoal = Goal;
            Vector2 oldStartingValue = StartingValue;
            Goal = oldStartingValue;
            StartingValue = oldGoal;
            if (timespan.HasValue)
                Length = timespan.Value;

            Reset();
        }
    }
