using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.Library;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;

namespace SlaamMono.Gameplay.Boards
{
    public class Tile
    {
        public TileCondition CurrentTileCondition { get => _state.CurrentTileCondition; }
        public Color MarkedColor { get => _state.MarkedColor; }
        public int MarkedIndex { get => _state.MarkedIndex; }
        public bool Dead { get => _state.Dead; }
        public PowerupType CurrentPowerupType { get => _state.CurrentPowerupType; }
        public float TimeTillClearing { get => _state.TimeTillClearing; }

        private TileState _state;

        private readonly IResources _resources;
        private readonly IRenderService _renderGraph;

        private readonly static TimeSpan FadeOutTime = new TimeSpan(0, 0, 0, 0, 25);


        public Tile(Vector2 Boardpos, Vector2 TileLoc, Texture2D tiletex, IResources resources, IRenderService renderGraph)
        {
            _resources = resources;
            _renderGraph = renderGraph;
            _state = IntializeState(Boardpos, TileLoc, tiletex, FadeOutTime);
        }
        public static TileState IntializeState(Vector2 Boardpos, Vector2 TileLoc, Texture2D tiletex, TimeSpan fadeOutTime)
        {
            TileState output;

            output = new TileState();
            output.ParentTileTileset = tiletex;
            output.TileCoors = TileLoc;
            output.AbsTileloc = new Vector2(Boardpos.X + TileLoc.X * GameGlobals.TILE_SIZE + 1, Boardpos.Y + TileLoc.Y * GameGlobals.TILE_SIZE + 1);
            output.FadeThrottle = new Timer(fadeOutTime);

            return output;
        }


        public void Update(GameScreenState gameScreenState) => updateState(gameScreenState, _state);
        public static void updateState(GameScreenState gameScreenState, TileState tileState)
        {
            if (tileState.CurrentTileCondition == TileCondition.RespawnPoint)
            {
                for (int x = 0; x < gameScreenState.Characters.Count; x++)
                {
                    if (x == tileState.MarkedIndex)
                    {
                        if (GameScreenFunctions.InterpretCoordinates(gameScreenState, gameScreenState.Characters[x].Position, true) != tileState.TileCoors)
                        {
                            tileState.CurrentTileCondition = TileCondition.Normal;
                        }
                    }
                }
                tileState.FallSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
                if (tileState.FallSpeed.Active)
                {
                    tileState.CurrentTileCondition = TileCondition.Normal;
                }
            }
            if (tileState.CurrentTileCondition == TileCondition.Marked)
            {
                tileState.FallSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
                if (tileState.FallSpeed.Active)
                {
                    tileState.CurrentTileCondition = TileCondition.Clearing;
                }
            }
            if (tileState.CurrentTileCondition == TileCondition.Clearing)
            {
                tileState.FadeThrottle.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
                if (tileState.FadeThrottle.Active)
                {
                    tileState.Alpha -= 12.75f;
                }

                tileState.TileColor = new Color((byte)255, (byte)255, (byte)255, (byte)tileState.Alpha);
                tileState.TileOverlayColor = new Color(tileState.MarkedColor.R, tileState.MarkedColor.G, tileState.MarkedColor.B, (byte)(tileState.Alpha / 2));
            }
            if (tileState.CurrentTileCondition == TileCondition.Clear && !tileState.Dead)
            {
                tileState.ReappearSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
                if (tileState.ReappearSpeed.Active)
                {
                    ResetTile(tileState);
                }
            }
            if (tileState.Alpha <= 0 && tileState.CurrentTileCondition == TileCondition.Clearing)
            {
                tileState.CurrentTileCondition = TileCondition.Clear;
                tileState.ReappearSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
            }
        }


        public void Draw(SpriteBatch batch) => drawState(batch, _state, _resources);
        public static void drawState(SpriteBatch batch, TileState tileState, IResources resources)
        {
            if (tileState.CurrentTileCondition != TileCondition.Clear)
            {

                batch.Draw(tileState.ParentTileTileset, tileState.AbsTileloc, new Rectangle((int)tileState.TileCoors.X * GameGlobals.TILE_SIZE, (int)tileState.TileCoors.Y * GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), tileState.TileColor);
                if (tileState.CurrentTileCondition != TileCondition.Normal)
                {
                    batch.Draw(resources.GetTexture("TileOverlay").Texture, new Rectangle((int)tileState.AbsTileloc.X, (int)tileState.AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), tileState.TileOverlayColor);
                }
                if (tileState.CurrentTileCondition == TileCondition.RespawnPoint)
                {
                    batch.Draw(resources.GetTexture("RespawnTileOverlay").Texture, new Rectangle((int)tileState.AbsTileloc.X, (int)tileState.AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), tileState.MarkedColor);
                }
                if (tileState.CurrentPowerupType != PowerupType.None)
                {
                    Texture2D tex = PowerupManager.Instance.GetPowerupTexture(tileState.CurrentPowerupType);
                    batch.Draw(tex, new Rectangle((int)tileState.AbsTileloc.X, (int)tileState.AbsTileloc.Y, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE), Color.White);
                }
            }
        }


        public void DrawShadow(SpriteBatch batch, TileState tileState)
        {
            drawShadow(tileState, _renderGraph);
        }
        private static void drawShadow(TileState tileState, IRenderService _renderGraph)
        {
            if (tileState.CurrentTileCondition != TileCondition.Clear)
            {
                _renderGraph.RenderBox(
                    destinationRectangle: new Rectangle((int)tileState.AbsTileloc.X + 10, (int)tileState.AbsTileloc.Y + 10, GameGlobals.TILE_SIZE, GameGlobals.TILE_SIZE),
                    color: new Color(0, 0, 0, 50));
            }
        }


        public void MarkTileForRespawn(Color markingcolor, TimeSpan Delay, int idx)
        {
            markTileForRespawn(_state, markingcolor, Delay, idx);
        }
        private static void markTileForRespawn(TileState tileState, Color markingcolor, TimeSpan Delay, int idx)
        {
            tileState.MarkedIndex = idx;
            tileState.MarkedColor = markingcolor;
            tileState.CurrentTileCondition = TileCondition.RespawnPoint;
            tileState.FallSpeed.Threshold = Delay;
            tileState.FallSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
            tileState.TileColor = Color.White;
        }


        public void MarkWithPowerup(PowerupType type)
        {
            markWithPowerup(_state, type);
        }
        private static void markWithPowerup(TileState tileState, PowerupType type)
        {
            tileState.CurrentPowerupType = type;
        }


        public void MarkTile(Color markingcolor, TimeSpan FallDelay, bool cominback, int IDX)
        {
            markTile(_state, markingcolor, FallDelay, cominback, IDX);
        }
        private static void markTile(TileState tileState, Color markingcolor, TimeSpan FallDelay, bool cominback, int IDX)
        {
            if (tileState.CurrentTileCondition == TileCondition.Normal || tileState.CurrentTileCondition == TileCondition.RespawnPoint && cominback)
            {
                tileState.MarkedIndex = IDX;
                tileState.MarkedColor = markingcolor;
                tileState.TileOverlayColor = new Color(markingcolor.R, markingcolor.G, markingcolor.B, (byte)127);
                tileState.CurrentTileCondition = TileCondition.Marked;
                tileState.FallSpeed.Threshold = FallDelay;
                tileState.FallSpeed.Update(FrameTimeService.Instance.GetLatestFrame().MovementFactorTimeSpan);
                tileState.TileColor = Color.White;
                tileState.Dead = cominback;
            }
            if (cominback)
            {
                tileState.Dead = cominback;
            }
        }


        public void ResetTileLocation(Vector2 Boardpos, Vector2 TileLoc)
        {
            resetTileLocation(_state, Boardpos, TileLoc);
        }
        private static void resetTileLocation(TileState tileState, Vector2 Boardpos, Vector2 TileLoc)
        {
            tileState.TileCoors = TileLoc;
            tileState.AbsTileloc = new Vector2(Boardpos.X + TileLoc.X * GameGlobals.TILE_SIZE + 1, Boardpos.Y + TileLoc.Y * GameGlobals.TILE_SIZE + 1);
        }


        private static void ResetTile(TileState tileState)
        {
            tileState.TileColor = Color.White;
            tileState.Alpha = 255;
            tileState.CurrentTileCondition = TileCondition.Normal;
            tileState.CurrentPowerupType = PowerupType.None;
            tileState.MarkedColor = Color.Transparent;
            tileState.FadeThrottle.Reset();
            tileState.FallSpeed.Reset();
            tileState.ReappearSpeed.Reset();
        }

    }
}
