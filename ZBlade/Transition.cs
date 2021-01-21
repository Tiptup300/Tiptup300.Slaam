using System;
using Microsoft.Xna.Framework;

namespace ZBlade
{
	internal class Transition
	{
		public Vector2 Position;
		public Vector2 Goal;
		public Vector2 StartingValue;
		public TimeSpan Length;
		public TimeSpan Elapsed;

		public Transition(Vector2 startingValue, Vector2 goal, TimeSpan length)
		{
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
		/// <summary>
		/// Updates the transition.
		/// </summary>
		/// <param name="elapsed"></param>
		/// <returns>If the position has changed.</returns>
		public bool Update(TimeSpan elapsed)
		{
			Vector2 Start = Position;
			Elapsed += elapsed;
			double amount = Elapsed.TotalSeconds / this.Length.TotalSeconds;
			Position = Vector2.SmoothStep(StartingValue, Goal, (float)amount);
			return Start != Position;

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
}
