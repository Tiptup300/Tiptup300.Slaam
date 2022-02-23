using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.SubClasses;
using System;
using ZzziveGameEngine;

namespace SlaamMono.Gameplay.Boards
{
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
        public Timer FadeThrottle;
        public Timer FallSpeed = new Timer(new TimeSpan(0, 0, 0, 0, 400));
        public Timer ReappearSpeed = new Timer(new TimeSpan(0, 0, 5));
        public float Alpha = 255;
    }
}
