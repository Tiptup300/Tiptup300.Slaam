using Microsoft.Xna.Framework;

namespace SlaamMono.Gameplay.Actors;

 public class BotTarget
 {
     public int PlayerIndex;
     public Vector2 Position;
     public float Distance;
     public TargetType ThisTargetType;

     public BotTarget(int playerindex, Vector2 position, float distance)
     {
         Position = position;
         Distance = distance;
         PlayerIndex = playerindex;
         ThisTargetType = TargetType.Character;
     }

     public BotTarget(Vector2 position, float distance)
     {
         Position = position;
         Distance = distance;
         PlayerIndex = -2;
         ThisTargetType = TargetType.Powerup;
     }

     public BotTarget(Vector2 position)
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
