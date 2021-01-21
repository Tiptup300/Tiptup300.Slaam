using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SlaamMono
{
    class Target
    {
        public int PlayerIndex;
        public Vector2 Position;
        public float Distance;
        public TargetType ThisTargetType;

        public Target(int playerindex, Vector2 position, float distance)
        {
            Position = position;
            Distance = distance;
            PlayerIndex = playerindex;
            ThisTargetType = TargetType.Character;
        }

        public Target(Vector2 position, float distance)
        {
            Position = position;
            Distance = distance;
            PlayerIndex = -2;
            ThisTargetType = TargetType.Powerup;
        }

        public Target(Vector2 position)
        {
            Position = position;
            PlayerIndex = -2;
            ThisTargetType = TargetType.Safety;
        }

        public enum TargetType
        {
            Safety,
            Character,
            Powerup,
        }

        public override string ToString()
        {
            return ThisTargetType.ToString();
        }
    }
}
