using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.States.Match.Boards;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.Match.Powerups;

namespace Tiptup300.Slaam.States.Match.Actors;

public class BotActor : CharacterActor
{
   private readonly Vector2 NULL_VECTOR_NEGATIVE_TWO = new Vector2(-2, -2);

   private BotTarget? attackTarget;
   private Direction direction = Direction.None;
   private bool isMovingTowardsSafety = false;
   private bool isSwitchingMovements = false;

   private readonly InputDevice _inputDevice = new InputDevice(InputDeviceType.Other, ExtendedPlayerIndex.Eight, -1);
   private readonly RandomList<int[]> _placesToGo = new RandomList<int[]>()
   {

      new int[] { 0, 1 },
      new int[] { 0, -1 },
      new int[] { 1, 0 },
      new int[] { -1, 0 }
   };
   private readonly TimerWidget _diagonalMovementSwitchTimer = new TimerWidget(new TimeSpan(0, 0, 0, 0, 500));
   private readonly TimerWidget _logicUpdateThresholdTimer = new TimerWidget(new TimeSpan(0, 0, 0, 0, 500));
   private readonly TimerWidget _targetTimer = new TimerWidget(new TimeSpan(0, 0, 5));
   private readonly IFrameTimeService _frameTimeService;
   private readonly Random _random = new Random();
   private GameConfiguration _gameConfiguration => ServiceLocator.Instance.GetService<GameConfiguration>();



   public BotActor(Texture2D skin, int profile, Vector2 pos, MatchPerformer parentgamescreen, Color markingcolor, int plyeridx, IResources resources,
       IFrameTimeService frameTimeService, MatchSettings matchSettings) :
       base(skin, profile, pos, null, markingcolor, plyeridx, resources, frameTimeService, matchSettings)
   {
      Gamepad = _inputDevice;
      _frameTimeService = frameTimeService;
      IsBot = true;

   }
   public override void Update(Vector2 CurrentCoordinates, Vector2 TilePos, MatchState gameScreenState)
   {
      _inputDevice.PressedAction2 = false;

      _diagonalMovementSwitchTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (_diagonalMovementSwitchTimer.Active)
      {
         isSwitchingMovements = _random.Next(0, 2) == 1;
      }

      _logicUpdateThresholdTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (_logicUpdateThresholdTimer.Active)
      {
         LogicUpdate(CurrentCoordinates, TilePos, gameScreenState);
         _logicUpdateThresholdTimer.Reset();
      }

      createInput();

      base.Update(CurrentCoordinates, TilePos, gameScreenState);

      // 2023-09-23-- mad:
      // 
      // this should not be down here, but I believe this is a bit of a bug fix.
      // where there's other parts of the system that read inputs on the input devices
      // so the lower implementation of botplayer  just checks input and we're using that
      // to actually steer the other character
      //
      // the original purpose of this was to make sure the ai didn't have any super powers
      // over a normal user, but it's just sort of clunky
      // 
      // instead of driving a fake controller and having the logic run on that, we should
      // be able to tell the player driver which actions we want to perform
      // which would be very similar to input devices, but not as mismatched 
      // of concerns.
      ClearInput();
   }

   private void LogicUpdate(Vector2 CurrentCoordinates, Vector2 TilePos, MatchState gameScreenState)
   {
      var currentTile = gameScreenState.Tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y];
      var isMoving = true;
      var isInDanger = currentTile.CurrentTileCondition != TileCondition.Normal && currentTile.CurrentTileCondition != TileCondition.RespawnPoint;

      if (CurrentState == CharacterState.Dead || CurrentState == CharacterState.Dieing)
      {
         // Dont Do Anything...your dead!
         isMoving = false;
      }
      else if (isInDanger)
      {
         if (HasPowerup(PowerupUse.Evasion))
            _inputDevice.PressedAction2 = true;

         // Not Safe need to do something
         if (isMovingTowardsSafety)
         {
            // Just needs to move
            if (attackTarget == null || CurrentCoordinates == attackTarget.Position)
               isMovingTowardsSafety = false;
         }

         if (!isMovingTowardsSafety)
         {
            isMovingTowardsSafety = true;

            var safePlaceToGo = findSafePlace(CurrentCoordinates, gameScreenState);

            if (safePlaceToGo == NULL_VECTOR_NEGATIVE_TWO)
            {
               attackTarget = null;
            }
            else
            {
               attackTarget = new BotTarget(safePlaceToGo);
            }
         }
      }
      else
      {
         if (isMovingTowardsSafety)
         {
            isMovingTowardsSafety = false;
         }
         var targets = new List<BotTarget>();

         for (int x = 0; x < gameScreenState.Characters.Count; x++)
         {
            if (x != _playerIndex &&
                 gameScreenState.Characters[x] != null &&
                 gameScreenState.Characters[x].CurrentState != CharacterState.Dead &&
                 gameScreenState.Characters[x].CurrentState != CharacterState.Dieing &&
                 gameScreenState.Characters[x].MarkingColor != MarkingColor)
            {
               var pos = MatchFunctions.InterpretCoordinates(gameScreenState, gameScreenState.Characters[x].Position, true);
               if (pos != CurrentCoordinates)
                  targets.Add(new BotTarget(x, pos, GetDistance(CurrentCoordinates, pos)));

            }
         }

         for (int x = 0; x < _gameConfiguration.BOARD_WIDTH; x++)
         {
            for (int y = 0; y < _gameConfiguration.BOARD_HEIGHT; y++)
            {
               if (gameScreenState.Tiles[x, y].CurrentPowerupType != PowerupType.None)
               {
                  float distance = GetDistance(new Vector2(x, y), CurrentCoordinates);
                  targets.Add(new BotTarget(new Vector2(x, y), distance));
               }
            }
         }

         var lowestDistance = 1000f;
         var savedTarget = -2;

         for (int x = 0; x < targets.Count; x++)
         {
            if (targets[x].Distance < lowestDistance)
            {
               savedTarget = x;
               lowestDistance = targets[x].Distance;
            }
         }

         if (savedTarget == -2)
         {
            isMoving = false;
            attackTarget = null;
         }
         else
         {
            attackTarget = targets[savedTarget];

            if (HasPowerup(PowerupUse.Strategy))
               _inputDevice.PressedAction2 = true;
         }
      }

      var isAttacking = attackTarget != null &&
          attackTarget.ThisTargetType == BotTarget.TargetType.Character &&
          gameScreenState.Characters[attackTarget.PlayerIndex].CurrentState != CharacterState.Dead &&
          gameScreenState.Characters[attackTarget.PlayerIndex].CurrentState != CharacterState.Dieing;

      if (isMoving)
      {
         MakeMovements(CurrentCoordinates, isAttacking, gameScreenState.Tiles);
      }
      else
      {
         direction = Direction.None;
      }
   }

   private void ClearInput()
   {
      _inputDevice.PressedAction = false;
      _inputDevice.PressedAction2 = false;
      _inputDevice.PressedBack = false;
      _inputDevice.PressedDown = false;
      _inputDevice.PressedLeft = false;
      _inputDevice.PressedRight = false;
      _inputDevice.PressedStart = false;
      _inputDevice.PressedUp = false;
      _inputDevice.PressingDown = false;
      _inputDevice.PressingLeft = false;
      _inputDevice.PressingRight = false;
      _inputDevice.PressingUp = false;
   }

   private bool CurrentTargetIsGood(List<CharacterActor> characterActors)
   {
      if (attackTarget != null && attackTarget.ThisTargetType == BotTarget.TargetType.Character)
      {
         if (!(characterActors[attackTarget.PlayerIndex].CurrentState == CharacterState.Dead) &&
             !(characterActors[attackTarget.PlayerIndex].CurrentState == CharacterState.Dieing))
         {
            _targetTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (_targetTimer.Active)
               _targetTimer.Reset();
            else
               return true;
         }
      }
      return false;
   }

   private void MakeMovements(Vector2 currentCoords, bool isAttacking, Tile[,] tiles)
   {

      if (attackTarget == null)
      {
         direction = Direction.None;
         return;
      }
      if (currentCoords.X != attackTarget.Position.X)
      {
         if (currentCoords.X > attackTarget.Position.X && IsSafe(tiles, currentCoords, -1, 0))
         {
            direction = Direction.Left;
         }
         else if (currentCoords.X < attackTarget.Position.X && IsSafe(tiles, currentCoords, 1, 0))
         {
            direction = Direction.Right;
         }
      }

      if (currentCoords.Y != attackTarget.Position.Y)
      {
         if (currentCoords.Y > attackTarget.Position.Y - 1)
         {
            if (direction == Direction.Left && IsSafe(tiles, currentCoords, -1, -1))
               direction = Direction.UpperLeft;
            else if (direction == Direction.Right && IsSafe(tiles, currentCoords, 1, -1))
               direction = Direction.UpperRight;
            else if (IsSafe(tiles, currentCoords, 0, -1))
               direction = Direction.Up;
         }
         else if (currentCoords.Y < attackTarget.Position.Y + 1)
         {
            if (direction == Direction.Left && IsSafe(tiles, currentCoords, -1, 1))
               direction = Direction.LowerLeft;
            else if (direction == Direction.Right && IsSafe(tiles, currentCoords, 1, 1))
               direction = Direction.LowerRight;
            else if (IsSafe(tiles, currentCoords, 0, 1))
               direction = Direction.Down;
         }
      }

      if (isAttacking)
      {
         if (HasAttackingPowerup(currentCoords))
            _inputDevice.PressedAction2 = true;
      }

      if (isAttacking && GetDistance(currentCoords, attackTarget.Position) <= 8f)
      { // Right Distance
         if (currentCoords.X == attackTarget.Position.X ||
             currentCoords.Y == attackTarget.Position.Y)
         { // Right Position

            _inputDevice.PressedAction = true;
         }
      }

   }

   private Vector2 findSafePlace(Vector2 CurrentCoordinates, MatchState gameScreenState)
   {
      _placesToGo.RandomizeList();

      Vector2 SafePlace = Vector2.Zero;

      bool FoundSafePlace = false;

      for (int x = 0; x < _placesToGo.Count; x++)
      {
         Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + _placesToGo[x][0], CurrentCoordinates.Y + _placesToGo[x][1]);
         if (IsSafeAndClear(CurrentTileLocation, gameScreenState))
         {
            SafePlace = new Vector2(CurrentTileLocation.X, CurrentTileLocation.Y);
            FoundSafePlace = true;
         }

      }

      if (FoundSafePlace)
      {
         return SafePlace;
      }
      else
      {
         float Highest = gameScreenState.Tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y].TimeTillClearing;

         for (int x = 0; x < _placesToGo.Count; x++)
         {
            Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + _placesToGo[x][0], CurrentCoordinates.Y + _placesToGo[x][1]);
            if (IsClear(CurrentTileLocation, gameScreenState))
            {
               float temp = gameScreenState.Tiles[(int)CurrentTileLocation.X, (int)CurrentTileLocation.Y].TimeTillClearing;

               if (temp > Highest)
               {
                  SafePlace = new Vector2(CurrentTileLocation.X, CurrentTileLocation.Y);
                  FoundSafePlace = true;
               }
            }
         }

         if (FoundSafePlace)
            return SafePlace;
         else
            return NULL_VECTOR_NEGATIVE_TWO;
      }
   }

   private bool HasPowerup(PowerupUse use)
   {
      if (CurrentPowerup != null && !CurrentPowerup.Used && CurrentPowerup.ThisPowerupsUse == use)
         return true;
      return false;
   }

   private bool HasAttackingPowerup(Vector2 CurrentCoordinates)
   {
      if (CurrentPowerup != null && !CurrentPowerup.Used && CurrentPowerup.ThisPowerupsUse == PowerupUse.Attacking)
      {
         if (GetDistance(CurrentCoordinates, attackTarget.Position) <= CurrentPowerup.AttackingRange)
         {
            if (CurrentPowerup.AttackingInLine)
            {
               if (CurrentCoordinates.X == attackTarget.Position.X ||
                   CurrentCoordinates.Y == attackTarget.Position.Y)
               {
                  return true;
               }
            }
            else
            {
               return true;
            }
         }
      }
      return false;

   }

   // likely should be moved into seperate BotInputDevice or AiInputDevice
   private void createInput()
   {
      _inputDevice.PressedLeft = false;
      _inputDevice.PressedRight = false;
      _inputDevice.PressedUp = false;
      _inputDevice.PressedDown = false;

      _inputDevice.PressingLeft = false;
      _inputDevice.PressingRight = false;
      _inputDevice.PressingUp = false;
      _inputDevice.PressingDown = false;

      if (direction == Direction.Left)
         _inputDevice.PressingLeft = true;
      else if (direction == Direction.Right)
         _inputDevice.PressingRight = true;
      else if (direction == Direction.Up)
         _inputDevice.PressingUp = true;
      else if (direction == Direction.Down)
         _inputDevice.PressingDown = true;
      else if (direction == Direction.LowerLeft)
      {
         if (isSwitchingMovements)
            _inputDevice.PressingDown = true;
         else
            _inputDevice.PressingLeft = true;
      }
      else if (direction == Direction.LowerRight)
      {
         if (isSwitchingMovements)
            _inputDevice.PressingDown = true;
         else
            _inputDevice.PressingRight = true;
      }
      else if (direction == Direction.UpperLeft)
      {
         if (isSwitchingMovements)
            _inputDevice.PressingUp = true;
         else
            _inputDevice.PressingLeft = true;
      }
      else if (direction == Direction.UpperRight)
      {
         if (isSwitchingMovements)
            _inputDevice.PressingUp = true;
         else
            _inputDevice.PressingRight = true;
      }
   }

   /// <summary>
   /// Calc's the distance from point A to B
   /// </summary>
   /// <param name="one">Point A</param>
   /// <param name="two">Point B</param>
   /// <returns></returns>
   public float GetDistance(Vector2 one, Vector2 two)
   {
      return (float)Math.Sqrt(Math.Abs(one.X - two.X) * Math.Abs(one.X - two.X) + Math.Abs(one.Y - two.Y) * Math.Abs(one.Y - two.Y));
   }

   private bool IsSafe(Tile[,] s, Vector2 Coords, int x, int y)
   {
      return IsSafe(new Vector2(Coords.X + x, Coords.Y + y), s);
   }

   private enum BotStatus
   {
      GoingTowardsEnemy,
      GoingTowardsSafety,
      Roaming,
   }

}
public enum Direction
{
   UpperLeft,
   Up,
   UpperRight,
   Left,
   None,
   Right,
   LowerLeft,
   Down,
   LowerRight,
}
