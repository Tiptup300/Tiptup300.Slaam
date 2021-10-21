using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Helpers;
using SlaamMono.Library.Input;
using SlaamMono.Powerups;
using SlaamMono.Resources;
using SlaamMono.Screens;
using System;

namespace SlaamMono.SubClasses
{
    public class Character
    {
        #region Variables

        public const float CharDrawScale = GameGlobals.TILE_SIZE / 45f;//0.67f;

        public bool Drawn = false;
        public int Lives = CurrentMatchSettings.LivesAmt;
        public int Kills = 0;
        public int PowerupsUsed = 0;
        public int Deaths = 0;

        public readonly float SpeedOfMovement = GameGlobals.TILE_SIZE / 50f * (5f / 30f) * CurrentMatchSettings.SpeedMultiplyer;

        public bool IsBot = false;
        public Texture2D CharacterSkin;
        public int ProfileIndex;
        public Vector2 Position;
        public InputDevice Gamepad;

        public Timer WalkingAnimationChange = new Timer(new TimeSpan(0, 0, 0, 0, 60));
        public Timer AttackingAnimationChange = new Timer(new TimeSpan(0, 0, 0, 0, (int)(GameGlobals.TILE_SIZE / 50f * 300)));
        private Timer ReappearTime = new Timer(CurrentMatchSettings.RespawnTime);

        public int Row;
        public SpriteEffects fx = SpriteEffects.None;
        public IntRange currAni = new IntRange(0, 0, 2);
        public bool currentlymoving;
        public Color MarkingColor;
        public CharacterState CurrentState = CharacterState.Normal;
        private Timer FadeThrottle = new Timer(new TimeSpan(0, 0, 0, 0, 25));
        public Color SpriteColor = Color.White;
        private float Alpha = 255;
        public int PlayerIndex;
        public float[] SpeedMultiplyer = new float[3];

        public TimeSpan TimeAlive = new TimeSpan();

        public Tile CurrentTile;

        public Powerup CurrentPowerup;

        #endregion

        #region Constructor

        public Character(Texture2D skin, int profileidx, Vector2 pos, InputDevice gamepad, Color markingcolor, int idx)
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
            for (int x = 0; x < SpeedMultiplyer.Length; x++)
                SpeedMultiplyer[x] = 1f;
        }

        #endregion

        #region Update

        public virtual void Update(Tile[,] tiles, Vector2 CurrentCoordinates, Vector2 TilePos)
        {
            CurrentTile = tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y];
#if ZUNE
            if (Gamepad.PressedAction2)
#else
            if (Gamepad.PressedStart)
#endif
                GameScreen.Instance.PauseGame(PlayerIndex);

            if (Lives > 0)
                TimeAlive += FrameRateDirector.MovementFactorTimeSpan;

            if (CurrentCoordinates.X >= GameGlobals.BOARD_WIDTH || CurrentCoordinates.Y >= GameGlobals.BOARD_HEIGHT || CurrentCoordinates.X < 0 || CurrentCoordinates.Y < 0)
                throw new Exception("Character Exited Bounds, Error Currently being worked on.");

            CheckForDeath(tiles, CurrentCoordinates);
            currentlymoving = false;

            if (tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y].CurrentPowerupType != PowerupType.None)
            {
                GetPowerup(tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y]);
            }

            if (CurrentPowerup != null && CurrentPowerup.Active)
                CurrentPowerup.UpdateAttack();

            if (CurrentState == CharacterState.Normal)
            {
                float Movement = FrameRateDirector.MovementFactor * SpeedOfMovement;

                for (int x = 0; x < SpeedMultiplyer.Length; x++)
                    Movement *= SpeedMultiplyer[x];

                WalkingAnimationChange.Update(FrameRateDirector.MovementFactorTimeSpan);

                if (Movement < 50f)
                {

                    if (Gamepad.PressingDown || Gamepad.PressedDown)
                    {
                        if (IsClear(Position, new Vector2(0, Movement)))
                            Position.Y += Movement;

                        Row = 0;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingUp || Gamepad.PressedUp)
                    {
                        //if (!(TilePos.Y < 1f + Movement && !IsClear(tiles, CurrentCoordinates, 0, -1)))
                        if (IsClear(Position, new Vector2(0, -Movement)))
                            Position.Y -= Movement;
                        Row = 2;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingLeft || Gamepad.PressedLeft)
                    {
                        //if (!(TilePos.X < 1f + Movement && !IsClear(tiles, CurrentCoordinates, -1, 0)))
                        if (IsClear(Position, new Vector2(-Movement, 0)))
                            Position.X -= Movement;

                        Row = 1;
                        fx = SpriteEffects.FlipHorizontally;
                        currentlymoving = true;
                    }
                    else if (Gamepad.PressingRight || Gamepad.PressedRight)
                    {
                        if (IsClear(Position, new Vector2(Movement, 0)))
                            Position.X += Movement;

                        Row = 1;
                        fx = SpriteEffects.None;
                        currentlymoving = true;
                    }
                }

                if (WalkingAnimationChange.Active && currentlymoving)
                    currAni.Add(1);
#if ZUNE
                if (Gamepad.PressedAction && CurrentState != CharacterState.Attacking)
#else
                if (Gamepad.PressedAction2 && CurrentState != CharacterState.Attacking)
#endif
                {
                    if (CurrentPowerup != null && !CurrentPowerup.Used && !CurrentPowerup.Active)
                    {
                        CurrentPowerup.BeginAttack(Position, Direction.Left);
                        PowerupsUsed++;

                        if (CurrentPowerup.AttackingType)
                        {
                            CurrentState = CharacterState.Attacking;
                            currAni = new IntRange(3, 3, 4);
                        }
                    }
                }
#if ZUNE
                if (Gamepad.PressedStart && CurrentState != CharacterState.Attacking)
#else
                if (Gamepad.PressedAction && CurrentState != CharacterState.Attacking)
#endif
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
                            CurrentPowerup.EndAttack();
                        }
                        else
                        {
                            switch (Row)
                            {
                                case 2:
                                    for (int y = (int)CurrentCoordinates.Y - 1; y >= 0 && y >= CurrentCoordinates.Y - 8; y--)
                                    {
                                        ct += 100;
                                        tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                    }
                                    break;

                                case 0:
                                    for (int y = (int)CurrentCoordinates.Y + 1; y < GameGlobals.BOARD_HEIGHT && y <= CurrentCoordinates.Y + 8; y++)
                                    {
                                        ct += 100;
                                        tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                    }
                                    break;

                                case 1:
                                    if (fx == SpriteEffects.FlipHorizontally)
                                        for (int x = (int)CurrentCoordinates.X - 1; x >= 0 && x >= CurrentCoordinates.X - 8; x--)
                                        {
                                            ct += 100;
                                            tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                        }
                                    else
                                        for (int x = (int)CurrentCoordinates.X + 1; x < GameGlobals.BOARD_WIDTH && x <= CurrentCoordinates.X + 8; x++)
                                        {
                                            ct += 100;
                                            tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, PlayerIndex);
                                        }
                                    break;
                            }
                        }
                        //tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y].MarkTile(new Color(255,0,0,126));
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
                    Alpha -= 10.625f;
                if (Alpha <= 0)
                {
                    ReportDeath(tiles, CurrentCoordinates);
                    if (CurrentPowerup != null && CurrentPowerup.Active & !CurrentPowerup.AttackingType)
                        CurrentPowerup.EndAttack();
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

        #endregion

        #region Draw

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            if (Lives == 0)
            {
                batch.Draw(CharacterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
                batch.Draw(ResourceManager.GetTexture("DeadChar").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
            }
            else if (CurrentState == CharacterState.Dead)
            {
                batch.Draw(CharacterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
                batch.Draw(ResourceManager.GetTexture("Waiting").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
            }
            else
                batch.Draw(CharacterSkin, pos, new Rectangle(currAni.Value * 50, Row * 60, 50, 60), SpriteColor, 0f, new Vector2(25, 50), 1f, fx, 0f);
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(CharacterSkin, Position, new Rectangle(currAni.Value * 50, Row * 60, 50, 60), SpriteColor, 0f, new Vector2(25, 50), CharDrawScale, fx, 0f);

        }

        #endregion

        #region Is Tile Clear Method

        /// <summary>
        /// Detects whether the selected tile is able to be walked over.
        /// </summary>
        /// <param name="tiles">Listing of all tiles on the board.</param>
        /// <param name="TileCoor">Current Coordinates of the player.</param>
        /// <param name="x">X Tile Position Offset</param>
        /// <param name="y">Y Tile Position Offset</param>
        /// <returns></returns>
        public bool IsClear(Vector2 CurrentCoords, Vector2 Movement)
        {
            Vector2 TilePosition = GameScreen.Instance.InterpretCoordinates(new Vector2(CurrentCoords.X + Movement.X, CurrentCoords.Y + Movement.Y), true);

            return IsClear(TilePosition);
        }

        public bool IsClear(Vector2 TilePosition)
        {
            if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= GameGlobals.BOARD_WIDTH || TilePosition.Y >= GameGlobals.BOARD_HEIGHT)
                return false;

            Tile tile = GameScreen.Instance.tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == Tile.TileCondition.Clear ||
                tile.CurrentTileCondition == Tile.TileCondition.Clearing ||
                tile.MarkedColor == MarkingColor && (tile.CurrentTileCondition == Tile.TileCondition.Clear || tile.CurrentTileCondition == Tile.TileCondition.Clearing || tile.CurrentTileCondition == Tile.TileCondition.Marked))
                return false;

            return true;
        }

        public bool IsSafeAndClear(Vector2 TilePosition)
        {
            if (!IsClear(TilePosition))
                return false;

            Tile tile = GameScreen.Instance.tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == Tile.TileCondition.Marked)
                return false;

            return true;
        }

        public bool IsSafe(Vector2 TilePosition)
        {
            if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= GameGlobals.BOARD_WIDTH || TilePosition.Y >= GameGlobals.BOARD_HEIGHT)
                return false;

            Tile tile = GameScreen.Instance.tiles[(int)TilePosition.X, (int)TilePosition.Y];

            if (tile.CurrentTileCondition == Tile.TileCondition.Marked)
                return false;

            return true;
        }

        #endregion

        private void GetPowerup(Tile currtile)
        {
            if (CurrentPowerup != null && !CurrentPowerup.Used)
            {
                if (CurrentPowerup.Active && !CurrentPowerup.AttackingType)
                    CurrentPowerup.EndAttack();
            }
            switch (currtile.CurrentPowerupType)
            {
                case PowerupType.SpeedUp:
                    CurrentPowerup = new SpeedUp(this);
                    break;

                case PowerupType.SpeedDown:
                    CurrentPowerup = new SpeedDown(GameScreen.Instance, PlayerIndex);
                    break;

                case PowerupType.Inversion:
                    CurrentPowerup = new Inversion(GameScreen.Instance, PlayerIndex);
                    break;

                case PowerupType.Slaam:
                    CurrentPowerup = new SlaamPowerup(GameScreen.Instance, this, PlayerIndex);
                    break;

                default:
                    throw new Exception();
            }
            currtile.MarkWithPowerup(PowerupType.None);
        }

        #region Profile Data Methods

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

        #endregion

        #region Death Methods

        /// <summary>
        /// Checks if the current player should die.
        /// </summary>
        /// <param name="tiles">Listing of all tiles on the board.</param>
        /// <param name="TileCoor">Current Coordinates of the player.</param>
        private void CheckForDeath(Tile[,] tiles, Vector2 TileCoor)
        {
            if (CurrentState != CharacterState.Dieing && CurrentState != CharacterState.Dead)
                if (
                    tiles[(int)TileCoor.X, (int)TileCoor.Y].CurrentTileCondition == Tile.TileCondition.Clearing ||
                    tiles[(int)TileCoor.X, (int)TileCoor.Y].CurrentTileCondition == Tile.TileCondition.Clear
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
        public void ReportDeath(Tile[,] tiles, Vector2 coors)
        {
            if (GameScreen.Instance.ThisGameType == GameType.Classic || GameScreen.Instance.ThisGameType == GameType.Survival)
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
        public void Respawn(Vector2 pos, Vector2 other)
        {
            Position = pos;
            currAni = new IntRange(0, 0, 2);
            WalkingAnimationChange.Reset();
            AttackingAnimationChange.Reset();
            ReappearTime.Reset();
            CurrentState = CharacterState.Normal;
            Alpha = 255;
            SpriteColor = new Color((byte)255, (byte)255, (byte)255, (byte)Alpha);
            GameScreen.Instance.tiles[(int)other.X, (int)other.Y].MarkTileForRespawn(MarkingColor, new TimeSpan(0, 0, 0, 8), PlayerIndex);
        }

        #endregion

        #region Enums

        public enum CharacterState
        {
            Normal,
            Attacking,
            Dieing,
            Dead,
            Respawning,
        }

        #endregion
    }
}
