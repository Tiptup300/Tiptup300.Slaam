using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.Input;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.States.Match.Boards;
using Tiptup300.Slaam.States.Match.Misc;
using Tiptup300.Slaam.States.Match.Powerups;
using Tiptup300.Slaam.x_;

namespace Tiptup300.Slaam.States.Match.Actors;

public class BotActor : CharacterActor
{
   private readonly Vector2 NULL_VECTOR_NEGATIVE_TWO = new Vector2(-2, -2);


   private InputDevice artificialInputDevice = new InputDevice(InputDeviceType.Other, ExtendedPlayerIndex.Eight, -1);
   private Direction currentDirection = Direction.None;

   private RandomList<int[]> placesToGo = new RandomList<int[]>();
   private bool goingTowardsSafety = false;
   private BotTarget currentTarget;
   private bool switchMovements = false;

   private readonly TimerWidget diagonalMovementSwitchTimer = new TimerWidget(new TimeSpan(0, 0, 0, 0, 500));
   private readonly TimerWidget logicUpdateThresholdTimer = new TimerWidget(new TimeSpan(0, 0, 0, 0, 500));
   private readonly TimerWidget targetTimer = new TimerWidget(new TimeSpan(0, 0, 5));

   private readonly IFrameTimeService _frameTimeService;
   private readonly Random random = new Random();

   public BotActor(Texture2D skin, int profile, Vector2 pos, MatchPerformer parentgamescreen, Color markingcolor, int plyeridx, IResources resources,
       IFrameTimeService frameTimeService, MatchSettings matchSettings) :
       base(skin, profile, pos, null, markingcolor, plyeridx, resources, frameTimeService, matchSettings)
   {
      Gamepad = artificialInputDevice;
      _frameTimeService = frameTimeService;
      IsBot = true;

      placesToGo.Add(new int[] { 0, 1 });
      placesToGo.Add(new int[] { 0, -1 });
      placesToGo.Add(new int[] { 1, 0 });
      placesToGo.Add(new int[] { -1, 0 });
   }
   public override void Update(Vector2 CurrentCoordinates, Vector2 TilePos, MatchState gameScreenState)
   {
      artificialInputDevice.PressedAction2 = false;

      diagonalMovementSwitchTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (diagonalMovementSwitchTimer.Active)
      {
         switchMovements = random.Next(0, 2) == 1;
      }

      logicUpdateThresholdTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
      if (logicUpdateThresholdTimer.Active)
      {
         LogicUpdate(CurrentCoordinates, TilePos, gameScreenState);
         logicUpdateThresholdTimer.Reset();
      }

      CreateInput();

      base.Update(CurrentCoordinates, TilePos, gameScreenState);

      ClearInput();
   }

   private void LogicUpdate(Vector2 CurrentCoordinates, Vector2 TilePos, MatchState gameScreenState)
   {
      Tile CurrentTile = gameScreenState.Tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y];
      bool Moving = true, Attacking = false, InDanger = CurrentTile.CurrentTileCondition != TileCondition.Normal && CurrentTile.CurrentTileCondition != TileCondition.RespawnPoint;
      if (CurrentState == CharacterState.Dead || CurrentState == CharacterState.Dieing)
      {
         // Dont Do Anything...your dead!
         Moving = false;
      }
      else if (InDanger)
      {
         if (HasPowerup(PowerupUse.Evasion))
            artificialInputDevice.PressedAction2 = true;

         // Not Safe need to do something
         if (goingTowardsSafety)
         {
            // Just needs to move
            if (currentTarget == null || CurrentCoordinates == currentTarget.Position)
               goingTowardsSafety = false;
         }

         if (!goingTowardsSafety)
         {
            goingTowardsSafety = true;

            Vector2 PlaceToGo = FindSafePlace(CurrentCoordinates, gameScreenState);

            if (PlaceToGo == NULL_VECTOR_NEGATIVE_TWO)
            {
               currentTarget = null;
            }
            else
            {
               currentTarget = new BotTarget(PlaceToGo);
            }
         }
         Attacking = false;
      }
      else
      {
         if (goingTowardsSafety)
         {
            goingTowardsSafety = false;
         }
         List<BotTarget> Targets = new List<BotTarget>();

         for (int x = 0; x < gameScreenState.Characters.Count; x++)
         {
            if (x != PlayerIndex &&
                 gameScreenState.Characters[x] != null &&
                 gameScreenState.Characters[x].CurrentState != CharacterState.Dead &&
                 gameScreenState.Characters[x].CurrentState != CharacterState.Dieing &&
                 gameScreenState.Characters[x].MarkingColor != MarkingColor)
            {
               Vector2 pos = MatchFunctions.InterpretCoordinates(gameScreenState, gameScreenState.Characters[x].Position, true);
               if (pos != CurrentCoordinates)
                  Targets.Add(new BotTarget(x, pos, GetDistance(CurrentCoordinates, pos)));

            }
         }

         for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
         {
            for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
            {
               if (gameScreenState.Tiles[x, y].CurrentPowerupType != PowerupType.None)
               {
                  float distance = GetDistance(new Vector2(x, y), CurrentCoordinates);
                  Targets.Add(new BotTarget(new Vector2(x, y), distance));
               }
            }
         }

         float LowestDistance = 1000;
         int SavedTarget = -2;

         for (int x = 0; x < Targets.Count; x++)
         {
            if (Targets[x].Distance < LowestDistance)
            {
               SavedTarget = x;
               LowestDistance = Targets[x].Distance;
            }
         }

         if (SavedTarget == -2)
         {
            Moving = false;
            currentTarget = null;
         }
         else
         {
            currentTarget = Targets[SavedTarget];

            if (HasPowerup(PowerupUse.Strategy))
               artificialInputDevice.PressedAction2 = true;
         }
      }

      Attacking = currentTarget != null &&
          currentTarget.ThisTargetType == BotTarget.TargetType.Character &&
          gameScreenState.Characters[currentTarget.PlayerIndex].CurrentState != CharacterState.Dead &&
          gameScreenState.Characters[currentTarget.PlayerIndex].CurrentState != CharacterState.Dieing;

      if (Moving)
      {
         MakeMovements(CurrentCoordinates, Attacking, gameScreenState.Tiles);
      }
      else
      {
         currentDirection = Direction.None;
      }
   }

   private void ClearInput()
   {
      artificialInputDevice.PressedAction = false;
      artificialInputDevice.PressedAction2 = false;
      artificialInputDevice.PressedBack = false;
      artificialInputDevice.PressedDown = false;
      artificialInputDevice.PressedLeft = false;
      artificialInputDevice.PressedRight = false;
      artificialInputDevice.PressedStart = false;
      artificialInputDevice.PressedUp = false;
      artificialInputDevice.PressingDown = false;
      artificialInputDevice.PressingLeft = false;
      artificialInputDevice.PressingRight = false;
      artificialInputDevice.PressingUp = false;
   }

   private bool CurrentTargetIsGood(List<CharacterActor> characterActors)
   {
      if (currentTarget != null && currentTarget.ThisTargetType == BotTarget.TargetType.Character)
      {
         if (!(characterActors[currentTarget.PlayerIndex].CurrentState == CharacterState.Dead) &&
             !(characterActors[currentTarget.PlayerIndex].CurrentState == CharacterState.Dieing))
         {
            targetTimer.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (targetTimer.Active)
               targetTimer.Reset();
            else
               return true;
         }
      }
      return false;
   }

   private void MakeMovements(Vector2 CurrentCoordinates, bool Attacking, Tile[,] tiles)
   {

      if (currentTarget == null)
      {
         currentDirection = Direction.None;
      }
      else
      {
         //CurrentDirection = Direction.None;
         if (CurrentCoordinates.X != currentTarget.Position.X)
         {
            if (CurrentCoordinates.X > currentTarget.Position.X && IsSafe(tiles, CurrentCoordinates, -1, 0))
            {
               currentDirection = Direction.Left;
            }
            else if (CurrentCoordinates.X < currentTarget.Position.X && IsSafe(tiles, CurrentCoordinates, 1, 0))
            {
               currentDirection = Direction.Right;
            }
         }

         if (CurrentCoordinates.Y != currentTarget.Position.Y)
         {
            if (CurrentCoordinates.Y > currentTarget.Position.Y - 1)
            {
               if (currentDirection == Direction.Left && IsSafe(tiles, CurrentCoordinates, -1, -1))
                  currentDirection = Direction.UpperLeft;
               else if (currentDirection == Direction.Right && IsSafe(tiles, CurrentCoordinates, 1, -1))
                  currentDirection = Direction.UpperRight;
               else if (IsSafe(tiles, CurrentCoordinates, 0, -1))
                  currentDirection = Direction.Up;
            }
            else if (CurrentCoordinates.Y < currentTarget.Position.Y + 1)
            {
               if (currentDirection == Direction.Left && IsSafe(tiles, CurrentCoordinates, -1, 1))
                  currentDirection = Direction.LowerLeft;
               else if (currentDirection == Direction.Right && IsSafe(tiles, CurrentCoordinates, 1, 1))
                  currentDirection = Direction.LowerRight;
               else if (IsSafe(tiles, CurrentCoordinates, 0, 1))
                  currentDirection = Direction.Down;
            }
         }

         if (Attacking)
         {
            if (HasAttackingPowerup(CurrentCoordinates))
               artificialInputDevice.PressedAction2 = true;
         }

         if (Attacking && GetDistance(CurrentCoordinates, currentTarget.Position) <= 8f)
         { // Right Distance
            if (CurrentCoordinates.X == currentTarget.Position.X ||
                CurrentCoordinates.Y == currentTarget.Position.Y)
            { // Right Position

               artificialInputDevice.PressedAction = true;
            }
         }
      }
   }

   private Vector2 FindSafePlace(Vector2 CurrentCoordinates, MatchState gameScreenState)
   {
      placesToGo.RandomizeList();

      Vector2 SafePlace = Vector2.Zero;

      bool FoundSafePlace = false;

      for (int x = 0; x < placesToGo.Count; x++)
      {
         Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + placesToGo[x][0], CurrentCoordinates.Y + placesToGo[x][1]);
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

         for (int x = 0; x < placesToGo.Count; x++)
         {
            Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + placesToGo[x][0], CurrentCoordinates.Y + placesToGo[x][1]);
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
         if (GetDistance(CurrentCoordinates, currentTarget.Position) <= CurrentPowerup.AttackingRange)
         {
            if (CurrentPowerup.AttackingInLine)
            {
               if (CurrentCoordinates.X == currentTarget.Position.X ||
                   CurrentCoordinates.Y == currentTarget.Position.Y)
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

   /// <summary>
   /// Takes in the current Directions and converts them into actual input.
   /// </summary>
   public void CreateInput()
   {
      artificialInputDevice.PressedLeft = false;
      artificialInputDevice.PressedRight = false;
      artificialInputDevice.PressedUp = false;
      artificialInputDevice.PressedDown = false;

      artificialInputDevice.PressingLeft = false;
      artificialInputDevice.PressingRight = false;
      artificialInputDevice.PressingUp = false;
      artificialInputDevice.PressingDown = false;

      if (currentDirection == Direction.Left)
         artificialInputDevice.PressingLeft = true;
      else if (currentDirection == Direction.Right)
         artificialInputDevice.PressingRight = true;
      else if (currentDirection == Direction.Up)
         artificialInputDevice.PressingUp = true;
      else if (currentDirection == Direction.Down)
         artificialInputDevice.PressingDown = true;
      else if (currentDirection == Direction.LowerLeft)
      {
         if (switchMovements)
            artificialInputDevice.PressingDown = true;
         else
            artificialInputDevice.PressingLeft = true;
      }
      else if (currentDirection == Direction.LowerRight)
      {
         if (switchMovements)
            artificialInputDevice.PressingDown = true;
         else
            artificialInputDevice.PressingRight = true;
      }
      else if (currentDirection == Direction.UpperLeft)
      {
         if (switchMovements)
            artificialInputDevice.PressingUp = true;
         else
            artificialInputDevice.PressingLeft = true;
      }
      else if (currentDirection == Direction.UpperRight)
      {
         if (switchMovements)
            artificialInputDevice.PressingUp = true;
         else
            artificialInputDevice.PressingRight = true;
      }

#if ZUNE
         AIInput.PressedStart = AIInput.PressedAction;
         AIInput.PressedAction = AIInput.PressedAction2;
         AIInput.PressedAction2 = false;
#endif
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
