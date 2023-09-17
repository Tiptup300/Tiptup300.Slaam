using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Powerups;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Gameplay.Boards;

public class TileState : IState
{
   public TileCondition CurrentTileCondition = TileCondition.Normal;
   public Color MarkedColor;
   public int MarkedIndex;
   public bool Dead = false;
   public PowerupType CurrentPowerupType = PowerupType.None;
   public float TimeTillClearing => (float)FallSpeed.TimeLeft.TotalMilliseconds;

   public Color TileColor = Color.White;
   public Color TileOverlayColor;
   public Vector2 AbsTileloc;
   public Vector2 TileCoors;
   public Texture2D ParentTileTileset;
   public SubClasses.Timer FadeThrottle;
   public SubClasses.Timer FallSpeed = new SubClasses.Timer(new TimeSpan(0, 0, 0, 0, 400));
   public SubClasses.Timer ReappearSpeed = new SubClasses.Timer(new TimeSpan(0, 0, 5));
   public float Alpha = 255;
}
