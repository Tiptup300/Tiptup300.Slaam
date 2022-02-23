using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Composition.x_;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.PlayerProfiles;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;
using System.Collections.Generic;

namespace SlaamMono.Gameplay.Actors
{
    public class CharacterActor
    {
        public float[] SpeedMultiplyer = new float[3];
        public CharacterState CurrentState = CharacterState.Normal;
        public TimeSpan TimeAlive = new TimeSpan();
        public int ProfileIndex;
        public Vector2 Position;
        public Color MarkingColor;
        public int Lives = MatchSettings.CurrentMatchSettings.LivesAmt;
        public int Kills = 0;
        public Powerup CurrentPowerup;
        public bool IsBot = false;
        public bool Drawn = false;

        protected InputDevice Gamepad;
        protected int PlayerIndex;

        private int PowerupsUsed = 0;
        private int Deaths = 0;
        private readonly float SpeedOfMovement = GameGlobals.TILE_SIZE / 50f * (5f / 30f) * MatchSettings.CurrentMatchSettings.SpeedMultiplyer;
        private Texture2D CharacterSkin;
        private Timer WalkingAnimationChange = new Timer(new TimeSpan(0, 0, 0, 0, 60));
        private Timer AttackingAnimationChange = new Timer(new TimeSpan(0, 0, 0, 0, (int)(GameGlobals.TILE_SIZE / 50f * 300)));
        private int Row;
        private SpriteEffects fx = SpriteEffects.None;
        private IntRange currAni = new IntRange(0, 0, 2);
        private bool currentlymoving;
        private Color SpriteColor = Color.White;

        private Timer ReappearTime = new Timer(MatchSettings.CurrentMatchSettings.RespawnTime);
        private Timer FadeThrottle = new Timer(new TimeSpan(0, 0, 0, 0, 25));
        private float Alpha = 255;

        private const float _characterDrawScale = GameGlobals.TILE_SIZE / 45f;


        private readonly IResources _resources;

        public CharacterActor(Texture2D skin, int profileidx, Vector2 pos, InputDevice gamepad, Color markingcolor, int idx, IResources resources)
        {
            WalkingAnimationChange.MakeUpTime = false;
            AttackingAnimationChange.MakeUpTime = false;
            ReappearTime.MakeUpTime = false;
            CharacterSkin = skin;
            ProfileIndex = profileidx;
            Position = pos;
            Gamepad = gamepad;
            MarkingColor = markingcolor;
            PlayerIndex = idx;
            _resources = resources;

            for (int x = 0; x < SpeedMultiplyer.Length; x++)
            {
                SpeedMultiplyer[x] = 1f;
            }
        }

        public virtual void Update(Vector2 CurrentCoordinates, Vector2 TilePos, GameScreenState gameScreenState)
        {

            if (Gamepad.PressedStart)
            {
                GameScreen.Instance.PauseGame(PlayerIndex);
            }

            if (Lives > 0)
            {
                TimeAlive += FrameRateDirector.MovementFactorTimeSpan;
            }

            if (CurrentCoordinates.X >= GameGlobals.BOARD_WIDTH || CurrentCoordinates.Y >= GameGlobals.BOARD_HEIGHT || CurrentCoordinates.X < 0 || CurrentCoordinates.Y < 0)
            {
                throw new Exception("Character Exited Bounds, Error Currently being worked on.");
            }

            CheckForDeath(gameScreenState.Tiles, CurrentCoordinates);
            currentlymoving = false;

            if (gameScreenState.Tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y].CurrentPowerupType != PowerupType.None)
            {
                GetPowerup(gameScreenState.Tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y], gameScreenState);
            }

            if (CurrentPowerup != null && CurrentPowerup.Active)
            {
                CurrentPowerup.UpdateAttack(gameScreenState);
            }

            if (CurrentState == CharacterState.Normal)
            {
                float Movement = FrameRateDirector.MovementFactor * SpeedOfMovement;

                for (int x = 0; x < SpeedMultiplyer.Length; x++)
                {
                    Movement *= SpeedMultiplyer[x];
                }

                WalkingAnimationChange.Update(FrameRateDirector.MovementFactorTimeSpan);

                if (Movement < 50f)
                {

                    if (Gamepad.PressingDown || Gamepad.PressedDown)
                    {
                        if (IsClear(Position, new Vector2(0, Movement), gameScreenState.Tiles))
                        {
                            Position.Y += Movement;
                        }

                        Row = 0;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingUp || Gamepad.PressedUp)
                    {
                        if (IsClear(Position, new Vector2(0, -Movement), gameScreenState.Tiles))
                        {
                            Position.Y -= Movement;
                        }
                        Row = 2;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingLeft || Gamepad.PressedLeft)
                    {
                        if (IsClear(Position, new Vector2(-Movement, 0), gameScreenState.Tiles))
                        {
                            Position.X -= Movement;
                        }

                        Row = 1;
                        fx = SpriteEffects.FlipHorizontally;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingRight || Gamepad.PressedRight)
                    {
                        if (IsClear(Position, new Vector2(Movement, 0), gameScreenState.Tiles))
                        {
                            Position.X += Movement;
                        }

                        Row = 1;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                }

                if (WalkingAnimationChange.Active && currentlymoving)
                {
                    currAni.Add(1);
                }

                if (Gamepad.PressedAction && CurrentState != CharacterState.Attacking)
                {
                    if (CurrentPowerup != null && !CurrentPowerup.Used && !CurrentPowerup.Active)
                    {
                        CurrentPowerup.BeginAttack(Position, Direction.Left, gameScreenState);
                        PowerupsUsed++;

                        if (CurrentPowerup.AttackingType)
                        {
                            CurrentState = CharacterState.Attacking;
                            currAni = new IntRange(3, 3, 4);
                        }
                    }
                }
                if (Gamepad.PressedStart && CurrentState != CharacterState.Attacking)
                {
                    CurrentState = CharacterState.Attacking;
                    currAni = new IntRange(3, 3, 4);
                    AttackingAnimationChange.Update(FrameRateDirector.MovementFactorTimeSpan);
                }

            }
            else if (CurrentState == CharacterState.Attacking)
            {
                AttackingAnimationChange.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (AttackingAnimationChange.Active)
                {
                    currAni.Add(1);

                    int ct = 0;

                    if (currAni.Value == 3)
                    {
                        if (CurrentPowerup != null && CurrentPowerup.AttackingType && CurrentPowerup.Active && !CurrentPowerup.Used)
                        {
                            CurrentPowerup.EndAttack(gameScreenState);
                        }
                        else
                        {
                            switch (Row)
                            {
                                case 2:
                                    for (int y = (int)CurrentCoordinates.Y - 1; y >= 0 && y >= CurrentCoordinates.Y - 8; y--)
                                    {
                                        ct += 100;
                                        gameScreenState.Tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                    }
                                    break;

                                case 0:
                                    for (int y = (int)CurrentCoordinates.Y + 1; y < GameGlobals.BOARD_HEIGHT && y <= CurrentCoordinates.Y + 8; y++)
                                    {
                                        ct += 100;
                                        gameScreenState.Tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                    }
                                    break;

                                case 1:
                                    if (fx == SpriteEffects.FlipHorizontally)
                                        for (int x = (int)CurrentCoordinates.X - 1; x >= 0 && x >= CurrentCoordinates.X - 8; x--)
                                        {
                                            ct += 100;
                                            gameScreenState.Tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                        }
                                    else
                                        for (int x = (int)CurrentCoordinates.X + 1; x < GameGlobals.BOARD_WIDTH && x <= CurrentCoordinates.X + 8; x++)
                                        {
                                            ct += 100;
                                            gameScreenState.Tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                        }
                                    break;
                            }
                        }
                        CurrentState = CharacterState.Normal;
                        currAni = new IntRange(0, 0, 2);
                    }
                }
            }
            else if (CurrentState == CharacterState.Dieing)
            {
                currAni.Value = 3;
                Row = 0;
                FadeThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (FadeThrottle.Active)
                {
                    Alpha -= 10.625f;
                }
                if (Alpha <= 0)
                {
                    ReportDeath(gameScreenState.Tiles, CurrentCoordinates, gameScreenState.GameType);
                    if (CurrentPowerup != null && CurrentPowerup.Active & !CurrentPowerup.AttackingType)
                    {
                        CurrentPowerup.EndAttack(gameScreenState);
                    }
                }
                SpriteColor = new Color((byte)255, (byte)255, (byte)255, (byte)Alpha);
            }
            else if (CurrentState == CharacterState.Dead && Lives > 0)
            {
                ReappearTime.Update(FrameRateDirector.MovementFactorTimeSpan);
                if (ReappearTime.Active)
                {
                    CurrentState = CharacterState.Respawning;
                }
            }
        }

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            if (Lives == 0)
            {
                batch.Draw(CharacterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
                batch.Draw(_resources.GetTexture("DeadChar").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
            }
            else if (CurrentState == CharacterState.Dead)
            {
                batch.Draw(CharacterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
                batch.Draw(_resources.GetTexture("Waiting").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
            }
            else
            {
                batch.Draw(CharacterSkin, pos, new Rectangle(currAni.Value * 50, Row * 60, 50, 60), SpriteColor, 0f, new Vector2(25, 50), 1f, fx, 0f);
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(CharacterSkin, Position, new Rectangle(currAni.Value * 50, Row * 60, 50, 60), SpriteColor, 0f, new Vector2(25, 50), _characterDrawScale, fx, 0f);
        }

        /// <summary>
        /// Detects whether the selected tile is able to be walked over.
        /// </summary>
        /// <param name="tiles">Listing of all tiles on the board.</param>
        /// <param name="TileCoor">Current Coordinates of the player.</param>
        /// <param name="x">X Tile Position Offset</param>
        /// <param name="y">Y Tile Position Offset</param>
        /// <returns></returns>
        public bool IsClear(Vector2 CurrentCoords, Vector2 Movement, Tile[,] tiles)
        {
            Vector2 TilePosition = GameScreen.Instance.InterpretCoordinates(new Vector2(CurrentCoords.X + Movement.X, CurrentCoords.Y + Movement.Y), true);

            return IsClear(TilePosition, tiles);
        }

        public bool IsClear(Vector2 TilePosition, Tile[,] tiles)
        {
            if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= GameGlobals.BOARD_WIDTH || TilePosition.Y >= GameGlobals.BOARD_HEIGHT)
                return false;

            Tile tile = tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == TileCondition.Clear ||
                tile.CurrentTileCondition == TileCondition.Clearing ||
                tile.MarkedColor == MarkingColor && (tile.CurrentTileCondition == TileCondition.Clear || tile.CurrentTileCondition == TileCondition.Clearing || tile.CurrentTileCondition == TileCondition.Marked))
                return false;

            return true;
        }

        public bool IsSafeAndClear(Vector2 TilePosition, Tile[,] tiles)
        {
            if (!IsClear(TilePosition, tiles))
                return false;

            Tile tile = tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == TileCondition.Marked)
                return false;

            return true;
        }

        public bool IsSafe(Vector2 TilePosition, Tile[,] tiles)
        {
            if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= GameGlobals.BOARD_WIDTH || TilePosition.Y >= GameGlobals.BOARD_HEIGHT)
                return false;

            Tile tile = tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == TileCondition.Marked)
                return false;

            return true;
        }

        private void GetPowerup(Tile currtile, GameScreenState gameScreenState)
        {
            if (CurrentPowerup != null && !CurrentPowerup.Used)
            {
                if (CurrentPowerup.Active && !CurrentPowerup.AttackingType)
                {
                    CurrentPowerup.EndAttack(gameScreenState);
                }
            }
            switch (currtile.CurrentPowerupType)
            {
                case PowerupType.SpeedUp:
                    CurrentPowerup = new SpeedUp(this, x_Di.Get<IResources>());
                    break;

                case PowerupType.SpeedDown:
                    CurrentPowerup = new SpeedDown(GameScreen.Instance, PlayerIndex, x_Di.Get<IResources>());
                    break;

                case PowerupType.Inversion:
                    CurrentPowerup = new Inversion(GameScreen.Instance, PlayerIndex, x_Di.Get<IResources>());
                    break;

                case PowerupType.Slaam:
                    CurrentPowerup = new SlaamPowerup(GameScreen.Instance, this, PlayerIndex, x_Di.Get<IResources>());
                    break;

                default:
                    throw new Exception();
            }
            currtile.MarkWithPowerup(PowerupType.None);
        }

        /// <summary>
        /// Gets the profile of the current character based on the ProfileIndex.
        /// </summary>
        /// <returns></returns>
        public PlayerProfile GetProfile()
        {
            return ProfileManager.AllProfiles[ProfileIndex];
        }

        /// <summary>
        /// Adds the Kills, Powerups, etc. to the profile for later saving.
        /// </summary>
        public void SaveProfileData()
        {
            ProfileManager.AllProfiles[ProfileIndex].TotalKills += Kills;
            ProfileManager.AllProfiles[ProfileIndex].TotalPowerups += PowerupsUsed;
            ProfileManager.AllProfiles[ProfileIndex].TotalGames += 1;
            ProfileManager.AllProfiles[ProfileIndex].TotalDeaths += Deaths;
        }

        /// <summary>
        /// Checks if the current player should die.
        /// </summary>
        /// <param name="tiles">Listing of all tiles on the board.</param>
        /// <param name="TileCoor">Current Coordinates of the player.</param>
        private void CheckForDeath(Tile[,] tiles, Vector2 TileCoor)
        {
            if (CurrentState != CharacterState.Dieing && CurrentState != CharacterState.Dead)
                if (
                    tiles[(int)TileCoor.X, (int)TileCoor.Y].CurrentTileCondition == TileCondition.Clearing ||
                    tiles[(int)TileCoor.X, (int)TileCoor.Y].CurrentTileCondition == TileCondition.Clear
                    )
                {
                    CurrentState = CharacterState.Dieing;
                    FadeThrottle.Update(FrameRateDirector.MovementFactorTimeSpan);
                }
        }

        /// <summary>
        /// Reports to the GameScreen that the player is dead and should give a killcount to the killer.
        /// </summary>
        /// <param name="tiles">Listing of all tiles on the board.</param>
        /// <param name="coors">Current Coordinates of the player.</param>
        public void ReportDeath(Tile[,] tiles, Vector2 coors, GameType gameType)
        {
            if (gameType == GameType.Classic || gameType == GameType.Survival)
                Lives--;

            Deaths++;

            GameScreen.Instance.ReportKilling(tiles[(int)coors.X, (int)coors.Y].MarkedIndex, PlayerIndex);

            CurrentState = CharacterState.Dead;
            ReappearTime.Update(FrameRateDirector.MovementFactorTimeSpan);
        }

        /// <summary>
        /// Resets Most Variables to prepare for a Respawn.
        /// </summary>
        /// <param name="pos">Respawn Position</param>
        /// <param name="other">Interpretted Respawn Position</param>
        public void Respawn(Vector2 pos, Vector2 other, Tile[,] tiles)
        {
            Position = pos;
            currAni = new IntRange(0, 0, 2);
            WalkingAnimationChange.Reset();
            AttackingAnimationChange.Reset();
            ReappearTime.Reset();
            CurrentState = CharacterState.Normal;
            Alpha = 255;
            SpriteColor = new Color((byte)255, (byte)255, (byte)255, (byte)Alpha);
            tiles[(int)other.X, (int)other.Y].MarkTileForRespawn(MarkingColor, new TimeSpan(0, 0, 0, 8), PlayerIndex);
        }

        public enum CharacterState
        {
            Normal,
            Attacking,
            Dieing,
            Dead,
            Respawning,
        }
    }
}
