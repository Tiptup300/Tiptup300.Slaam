using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.Library;
using SlaamMono.Library.Resources;
using SlaamMono.Resources;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;

namespace SlaamMono.Gameplay.Boards
{
    public class Tile
    {

        private Vector2 AbsTileloc;
        private Vector2 TileCoors;
        private Texture2D ParentTileTileset;
        public TileCondition CurrentTileCondition = TileCondition.Normal;
        private static TimeSpan FadeOutTime = new TimeSpan(0, 0, 0, 0, 25);
        public Color TileColor = Color.White;
        public Color MarkedColor;
        public int MarkedIndex;
        public Color TileOverlayColor;
        private Timer FadeThrottle = new Timer(FadeOutTime);
        private Timer FallSpeed = new Timer(new TimeSpan(0, 0, 0, 0, 400));
        private Timer ReappearSpeed = new Timer(new TimeSpan(0, 0, 5));
        public bool Dead = false;
        public PowerupType CurrentPowerupType = PowerupType.None;

        private float Alpha = 255;

        public float TimeTillClearing
        {
            get
            {
                return (float)FallSpeed.TimeLeft.TotalMilliseconds;
            }
        }

        private IWhitePixelResolver _whitePixelResolver;

        public Tile(Vector2 Boardpos, Vector2 TileLoc, Texture2D tiletex)
        {
            ParentTileTileset = tiletex;
            TileCoors = TileLoc;
            AbsTileloc = new Vector2(Boardpos.X + TileLoc.X * GameGlobals.TILE_SIZE + 1, Boardpos.Y + TileLoc.Y * GameGlobals.TILE_SIZE + 1);

            x_initWhitePixel();
        }


        public Tile(Tile tile, Texture2D parenttiletex)
        {
            ParentTileTileset = parenttiletex;
            AbsTileloc = tile.AbsTileloc;

            x_initWhitePixel();
        }
        private void x_initWhitePixel()
        {
            _whitePixelResolver = DiImplementer.Instance.Get<IWhitePixelResolver>();
        }

        public void Update()
        {
            if (CurrentTileCondition == TileCondition.RespawnPoint)
            {
                for (int x = 0; x < GameScreen.Instance.Characters.Count; x++)
                {
                    if (x == MarkedIndex)
                    {
                        if (GameScreen.Instance.InterpretCoordinates(GameScreen.Instance.Characters[x].Position, true) != TileCoors)
                        {
                            CurrentTileCondition = TileCondition.Normal;
                        }
                    }
                }
                FallSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (FallSpeed.Active)
                    CurrentTileCondition = TileCondition.Normal;
            }
            if (CurrentTileCondition == TileCondition.Marked)
            {
                FallSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (FallSpeed.Active)
                    CurrentTileCondition = TileCondition.Clearing;
            }
            if (CurrentTileCondition == TileCondition.Clearing)
            {
                FadeThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (FadeThrottle.Active)
                    Alpha -= 12.75f;

                TileColor = new Color((byte)255, (byte)255, (byte)255, (byte)Alpha);
                TileOverlayColor = new Color(MarkedColor.R, MarkedColor.G, MarkedColor.B, (byte)(Alpha / 2));
            }
            if (CurrentTileCondition == TileCondition.Clear && !Dead)
            {
                ReappearSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (ReappearSpeed.Active)
                    ResetTile();
            }
            if (Alpha <= 0 && CurrentTileCondition == TileCondition.Clearing)
            {
                CurrentTileCondition = TileCondition.Clear;
                ReappearSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (CurrentTileCondition != TileCondition.Clear)
            {

                batch.Draw(ParentTileTileset, AbsTileloc, new Rectangle((int)TileCoors.X * GameGlobals.TILE_SIZE, (int)TileCoors.Y * GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), TileColor);
                if (CurrentTileCondition != TileCondition.Normal)
                    batch.Draw(ResourceManager.Instance.GetTexture("TileOverlay").Texture, new Rectangle((int)AbsTileloc.X, (int)AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), TileOverlayColor);
                if (CurrentTileCondition == TileCondition.RespawnPoint)
                    batch.Draw(ResourceManager.Instance.GetTexture("RespawnTileOverlay").Texture, new Rectangle((int)AbsTileloc.X, (int)AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), MarkedColor);
                if (CurrentPowerupType != PowerupType.None)
                {
                    Texture2D tex = PowerupManager.GetPowerupTexture(CurrentPowerupType);
                    batch.Draw(tex, new Rectangle((int)AbsTileloc.X, (int)AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), Color.White);
                }
            }
        }

        public void DrawShadow(SpriteBatch batch)
        {
            if (CurrentTileCondition != TileCondition.Clear)
            {
                batch.Draw(_whitePixelResolver.GetWhitePixel(), new Rectangle((int)AbsTileloc.X + 10, (int)AbsTileloc.Y + 10, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), new Color(0, 0, 0, 50));
            }
        }

        /// <summary>
        /// Marks the current tile for respawn so its invincible.
        /// </summary>
        /// <param name="markingcolor">Color to mark it.</param>
        /// <param name="Delay">How long do we delay it?</param>
        /// <param name="idx">Player's Index</param>
        public void MarkTileForRespawn(Color markingcolor, TimeSpan Delay, int idx)
        {
            MarkedIndex = idx;
            MarkedColor = markingcolor;
            CurrentTileCondition = TileCondition.RespawnPoint;
            FallSpeed.Threshold = Delay;
            FallSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
            TileColor = Color.White;
        }

        public void MarkWithPowerup(PowerupType type)
        {
            CurrentPowerupType = type;
        }

        /// <summary>
        /// Mark tile for killing
        /// </summary>
        /// <param name="markingcolor">Color to mark it.</param>
        /// <param name="FallDelay">How long do we dealy it?</param>
        /// <param name="cominback">Is it coming back?</param>
        /// <param name="IDX">Player's Index</param>
        public void MarkTile(Color markingcolor, TimeSpan FallDelay, bool cominback, int IDX)
        {
            if (CurrentTileCondition == TileCondition.Normal || CurrentTileCondition == TileCondition.RespawnPoint && cominback)
            {
                MarkedIndex = IDX;
                MarkedColor = markingcolor;
                TileOverlayColor = new Color(markingcolor.R, markingcolor.G, markingcolor.B, (byte)127);
                CurrentTileCondition = TileCondition.Marked;
                FallSpeed.Threshold = FallDelay;
                FallSpeed.Update(FrameRateDirector.MovementFactorTimeSpan);
                TileColor = Color.White;
                Dead = cominback;
            }
            if (cominback)
                Dead = cominback;
        }

        /// <summary>
        /// Resets real location of tile on screen.
        /// </summary>
        /// <param name="Boardpos"></param>
        /// <param name="TileLoc"></param>
        public void ResetTileLoc(Vector2 Boardpos, Vector2 TileLoc)
        {
            TileCoors = TileLoc;
            AbsTileloc = new Vector2(Boardpos.X + TileLoc.X * GameGlobals.TILE_SIZE + 1, Boardpos.Y + TileLoc.Y * GameGlobals.TILE_SIZE + 1);
        }

        /// <summary>
        /// Reset all tile variables for renewal
        /// </summary>
        private void ResetTile()
        {
            TileColor = Color.White;
            Alpha = 255;
            CurrentTileCondition = TileCondition.Normal;
            CurrentPowerupType = PowerupType.None;
            MarkedColor = Color.Transparent;
            FadeThrottle.Reset();
            FallSpeed.Reset();
            ReappearSpeed.Reset();
        }

        public enum TileCondition
        {
            Normal,
            RespawnPoint,
            Marked,
            Clearing,
            Clear
        }
    }
}
