using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.States.Match.Powerups;

namespace Tiptup300.Slaam.States.Match.Boards;

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
   public Library.Widgets.TimerWidget FadeThrottle;
   public Library.Widgets.TimerWidget FallSpeed = new Library.Widgets.TimerWidget(new TimeSpan(0, 0, 0, 0, 400));
   public Library.Widgets.TimerWidget ReappearSpeed = new Library.Widgets.TimerWidget(new TimeSpan(0, 0, 5));
   public float Alpha = 255;
}
