using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.Match.Boards;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.Match.Powerups;

namespace Tiptup300.Slaam.States.Match.Actors;

public class CharacterActor
{
   private const float CHARACTER_DRAW_SCALE = 1 / 45f;

   public float[] SpeedMultiplyer = new float[3];
   public CharacterState CurrentState = CharacterState.Normal;
   public TimeSpan TimeAlive = new TimeSpan();
   public int ProfileIndex;
   public Vector2 Position;
   public Color MarkingColor;
   public int Lives;
   public int Kills = 0;
   public Powerup CurrentPowerup;
   public bool IsBot = false;
   public bool Drawn = false;

   protected InputDevice Gamepad;

   private int powerupsUsed = 0;
   private int deaths = 0;
   private int row;
   private SpriteEffects fx = SpriteEffects.None;
   private IntRange currAni = new IntRange(0, 0, 2);
   private bool currentlymoving;
   private Color _spriteColor = Color.White;
   private float alpha = 255;


   private readonly float _speedOfMovement;
   private readonly Texture2D _characterSkin;
   private readonly TimerWidget _walkingAnimationChange = new TimerWidget(new TimeSpan(0, 0, 0, 0, 60));
   private readonly TimerWidget _attackingAnimationChange;
   private readonly TimerWidget _reappearTime = new TimerWidget();
   private readonly TimerWidget _fadeThrottle = new TimerWidget(new TimeSpan(0, 0, 0, 0, 25));
   protected readonly int _playerIndex;



   private readonly IResources _resources;
   private readonly IFrameTimeService _frameTimeService;
   private GameConfiguration _gameConfiguration => ServiceLocator.Instance.GetService<GameConfiguration>();

   public CharacterActor(Texture2D skin, int profileidx, Vector2 pos, InputDevice gamepad, Color markingcolor, int idx, IResources resources,
       IFrameTimeService frameTimeService, MatchSettings matchSettings)
   {
      _walkingAnimationChange.MakeUpTime = false;
      _attackingAnimationChange = new TimerWidget(new TimeSpan(0, 0, 0, 0, (int)(_gameConfiguration.TILE_SIZE / 50f * 300)));
      _attackingAnimationChange.MakeUpTime = false;
      _characterSkin = skin;
      ProfileIndex = profileidx;
      Position = pos;
      Gamepad = gamepad;
      MarkingColor = markingcolor;
      _playerIndex = idx;
      _resources = resources;
      _frameTimeService = frameTimeService;
      for (int x = 0; x < SpeedMultiplyer.Length; x++)
      {
         SpeedMultiplyer[x] = 1f;
      }
      Lives = matchSettings.LivesAmt;
      _speedOfMovement = _gameConfiguration.TILE_SIZE / 50f * (5f / 30f) * matchSettings.SpeedMultiplyer;
      _reappearTime.MakeUpTime = false;
      _reappearTime.Threshold = matchSettings.RespawnTime;

   }

   public virtual void Update(Vector2 CurrentCoordinates, Vector2 TilePos, MatchState gameScreenState)
   {

      if (Gamepad.PressedStart)
      {
         gameScreenState.IsPaused = true;
      }

      if (Lives > 0)
      {
         TimeAlive += _frameTimeService.GetLatestFrame().MovementFactorTimeSpan;
      }

      if (CurrentCoordinates.X >= _gameConfiguration.BOARD_WIDTH || CurrentCoordinates.Y >= _gameConfiguration.BOARD_HEIGHT || CurrentCoordinates.X < 0 || CurrentCoordinates.Y < 0)
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
         float Movement = _frameTimeService.GetLatestFrame().MovementFactor * _speedOfMovement;

         for (int x = 0; x < SpeedMultiplyer.Length; x++)
         {
            Movement *= SpeedMultiplyer[x];
         }

         _walkingAnimationChange.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);

         if (Movement < 50f)
         {

            if (Gamepad.PressingDown || Gamepad.PressedDown)
            {
               if (IsClear(Position, new Vector2(0, Movement), gameScreenState))
               {
                  Position.Y += Movement;
               }

               row = 0;
               fx = SpriteEffects.None;
               currentlymoving = true;
            }
            else if (Gamepad.PressingUp || Gamepad.PressedUp)
            {
               if (IsClear(Position, new Vector2(0, -Movement), gameScreenState))
               {
                  Position.Y -= Movement;
               }
               row = 2;
               fx = SpriteEffects.None;
               currentlymoving = true;
            }
            else if (Gamepad.PressingLeft || Gamepad.PressedLeft)
            {
               if (IsClear(Position, new Vector2(-Movement, 0), gameScreenState))
               {
                  Position.X -= Movement;
               }

               row = 1;
               fx = SpriteEffects.FlipHorizontally;
               currentlymoving = true;
            }
            else if (Gamepad.PressingRight || Gamepad.PressedRight)
            {
               if (IsClear(Position, new Vector2(Movement, 0), gameScreenState))
               {
                  Position.X += Movement;
               }

               row = 1;
               fx = SpriteEffects.None;
               currentlymoving = true;
            }
         }

         if (_walkingAnimationChange.Active && currentlymoving)
         {
            currAni.Add(1);
         }

         if (Gamepad.PressedAction && CurrentState != CharacterState.Attacking)
         {
            if (CurrentPowerup != null && !CurrentPowerup.Used && !CurrentPowerup.Active)
            {
               CurrentPowerup.BeginAttack(Position, Direction.Left, gameScreenState);
               powerupsUsed++;

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
            _attackingAnimationChange.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         }

      }
      else if (CurrentState == CharacterState.Attacking)
      {
         _attackingAnimationChange.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         if (_attackingAnimationChange.Active)
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
                  switch (row)
                  {
                     case 2:
                        for (int y = (int)CurrentCoordinates.Y - 1; y >= 0 && y >= CurrentCoordinates.Y - 8; y--)
                        {
                           ct += 100;
                           gameScreenState.Tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, _playerIndex);
                        }
                        break;

                     case 0:
                        for (int y = (int)CurrentCoordinates.Y + 1; y < _gameConfiguration.BOARD_HEIGHT && y <= CurrentCoordinates.Y + 8; y++)
                        {
                           ct += 100;
                           gameScreenState.Tiles[(int)CurrentCoordinates.X, y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, _playerIndex);
                        }
                        break;

                     case 1:
                        if (fx == SpriteEffects.FlipHorizontally)
                           for (int x = (int)CurrentCoordinates.X - 1; x >= 0 && x >= CurrentCoordinates.X - 8; x--)
                           {
                              ct += 100;
                              gameScreenState.Tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, _playerIndex);
                           }
                        else
                           for (int x = (int)CurrentCoordinates.X + 1; x < _gameConfiguration.BOARD_WIDTH && x <= CurrentCoordinates.X + 8; x++)
                           {
                              ct += 100;
                              gameScreenState.Tiles[x, (int)CurrentCoordinates.Y].MarkTile(MarkingColor, new TimeSpan(0, 0, 0, 0, 300 + ct), false, _playerIndex);
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
         row = 0;
         _fadeThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         if (_fadeThrottle.Active)
         {
            alpha -= 10.625f;
         }
         if (alpha <= 0)
         {
            ReportDeath(gameScreenState.Tiles, CurrentCoordinates, gameScreenState.GameType, gameScreenState);
            if (CurrentPowerup != null && CurrentPowerup.Active & !CurrentPowerup.AttackingType)
            {
               CurrentPowerup.EndAttack(gameScreenState);
            }
         }
         _spriteColor = new Color((byte)255, (byte)255, (byte)255, (byte)alpha);
      }
      else if (CurrentState == CharacterState.Dead && Lives > 0)
      {
         _reappearTime.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         if (_reappearTime.Active)
         {
            CurrentState = CharacterState.Respawning;
         }
      }
   }

   public void Draw(SpriteBatch batch, Vector2 pos)
   {
      if (Lives == 0)
      {
         batch.Draw(_characterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
         batch.Draw(_resources.GetTexture("DeadChar").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
      }
      else if (CurrentState == CharacterState.Dead)
      {
         batch.Draw(_characterSkin, pos, new Rectangle(0, 0, 50, 60), new Color(255, 255, 255, 140), 0f, new Vector2(25, 50), 1f, SpriteEffects.None, 0f);
         batch.Draw(_resources.GetTexture("Waiting").Texture, pos, new Rectangle(0, 0, 50, 60), Color.White, 0f, new Vector2(25, 50), 1f, fx, 0f);
      }
      else
      {
         batch.Draw(_characterSkin, pos, new Rectangle(currAni.Value * 50, row * 60, 50, 60), _spriteColor, 0f, new Vector2(25, 50), 1f, fx, 0f);
      }
   }

   public virtual void Draw(SpriteBatch batch)
   {
      batch.Draw(_characterSkin, Position, new Rectangle(currAni.Value * 50, row * 60, 50, 60), _spriteColor, 0f, new Vector2(25, 50), _gameConfiguration.TILE_SIZE * CHARACTER_DRAW_SCALE, fx, 0f);
   }

   /// <summary>
   /// Detects whether the selected tile is able to be walked over.
   /// </summary>
   /// <param name="tiles">Listing of all tiles on the board.</param>
   /// <param name="TileCoor">Current Coordinates of the player.</param>
   /// <param name="x">X Tile Position Offset</param>
   /// <param name="y">Y Tile Position Offset</param>
   /// <returns></returns>
   public bool IsClear(Vector2 CurrentCoords, Vector2 Movement, MatchState gameScreenState)
   {
      Vector2 TilePosition = MatchFunctions.InterpretCoordinates(gameScreenState, new Vector2(CurrentCoords.X + Movement.X, CurrentCoords.Y + Movement.Y), true);

      return IsClear(TilePosition, gameScreenState);
   }

   public bool IsClear(Vector2 TilePosition, MatchState gameScreenState)
   {
      if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= _gameConfiguration.BOARD_WIDTH || TilePosition.Y >= _gameConfiguration.BOARD_HEIGHT)
         return false;

      Tile tile = gameScreenState.Tiles[(int)TilePosition.X, (int)TilePosition.Y];

      if (tile.CurrentTileCondition == TileCondition.Clear ||
          tile.CurrentTileCondition == TileCondition.Clearing ||
          tile.MarkedColor == MarkingColor && (tile.CurrentTileCondition == TileCondition.Clear || tile.CurrentTileCondition == TileCondition.Clearing || tile.CurrentTileCondition == TileCondition.Marked))
         return false;

      return true;
   }

   public bool IsSafeAndClear(Vector2 TilePosition, MatchState gameScreenState)
   {
      if (!IsClear(TilePosition, gameScreenState))
         return false;

      Tile tile = gameScreenState.Tiles[(int)TilePosition.X, (int)TilePosition.Y];

      if (tile.CurrentTileCondition == TileCondition.Marked)
         return false;

      return true;
   }

   public bool IsSafe(Vector2 TilePosition, Tile[,] tiles)
   {
      if (TilePosition.X < 0 || TilePosition.Y < 0 || TilePosition.X >= _gameConfiguration.BOARD_WIDTH || TilePosition.Y >= _gameConfiguration.BOARD_HEIGHT)
         return false;

      Tile tile = tiles[(int)TilePosition.X, (int)TilePosition.Y];

      if (tile.CurrentTileCondition == TileCondition.Marked)
         return false;

      return true;
   }

   private void GetPowerup(Tile currtile, MatchState gameScreenState)
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
            CurrentPowerup = new SpeedUp(this, ServiceLocator.Instance.GetService<IResources>(), _frameTimeService);
            break;

         case PowerupType.SpeedDown:
            CurrentPowerup = new SpeedDown(_playerIndex, ServiceLocator.Instance.GetService<IResources>(), _frameTimeService);
            break;

         case PowerupType.Inversion:
            CurrentPowerup = new Inversion(_playerIndex, ServiceLocator.Instance.GetService<IResources>(), _frameTimeService);
            break;

         case PowerupType.Slaam:
            CurrentPowerup = new SlaamPowerup(this, _playerIndex, ServiceLocator.Instance.GetService<IResources>());
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
      return ProfileManager.Instance.State_AllProfiles[ProfileIndex];
   }

   /// <summary>
   /// Adds the Kills, Powerups, etc. to the profile for later saving.
   /// </summary>
   public void SaveProfileData()
   {
      ProfileManager.Instance.State_AllProfiles[ProfileIndex].TotalKills += Kills;
      ProfileManager.Instance.State_AllProfiles[ProfileIndex].TotalPowerups += powerupsUsed;
      ProfileManager.Instance.State_AllProfiles[ProfileIndex].TotalGames += 1;
      ProfileManager.Instance.State_AllProfiles[ProfileIndex].TotalDeaths += deaths;
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
            _fadeThrottle.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
         }
   }

   /// <summary>
   /// Reports to the GameScreen that the player is dead and should give a killcount to the killer.
   /// </summary>
   /// <param name="tiles">Listing of all tiles on the board.</param>
   /// <param name="coors">Current Coordinates of the player.</param>
   public void ReportDeath(Tile[,] tiles, Vector2 coors, GameType gameType, MatchState gameScreenState)
   {
      if (gameType == GameType.Classic || gameType == GameType.Survival)
         Lives--;

      deaths++;

      MatchFunctions.ReportKilling(tiles[(int)coors.X, (int)coors.Y].MarkedIndex, _playerIndex, gameScreenState, _frameTimeService);

      CurrentState = CharacterState.Dead;
      _reappearTime.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
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
      _walkingAnimationChange.Reset();
      _attackingAnimationChange.Reset();
      _reappearTime.Reset();
      CurrentState = CharacterState.Normal;
      alpha = 255;
      _spriteColor = new Color((byte)255, (byte)255, (byte)255, (byte)alpha);
      tiles[(int)other.X, (int)other.Y].MarkTileForRespawn(MarkingColor, new TimeSpan(0, 0, 0, 8), _playerIndex);
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
