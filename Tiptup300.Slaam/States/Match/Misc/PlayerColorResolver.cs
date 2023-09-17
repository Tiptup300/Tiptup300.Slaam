using Microsoft.Xna.Framework;

namespace SlaamMono.Gameplay;

 public class PlayerColorResolver
 {
     private readonly Color[] _playerColors = new Color[] {
         Color.Red,
         Color.Blue,
         Color.Green,
         Color.Yellow,
         Color.Cyan,
         Color.Orange,
         Color.Purple,
         Color.Pink
     };

     public Color GetColorByIndex(int playerIndex) => _playerColors[playerIndex];
 }
