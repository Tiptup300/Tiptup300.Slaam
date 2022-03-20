using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Gameplay.Powerups;
using SlaamMono.Library;
using SlaamMono.Library.Input;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.SubClasses;
using SlaamMono.x_;
using System;
using System.Collections.Generic;

namespace SlaamMono.Gameplay.Actors
{
    public class BotActor : CharacterActor
    {
        GameScreenPerformer ParentGameScreen;
        private readonly IFrameTimeService _frameTimeService;
        InputDevice AIInput = new InputDevice(InputDeviceType.Other, ExtendedPlayerIndex.Eight, -1);
        Timer DiagonalMovementSwitch = new Timer(new TimeSpan(0, 0, 0, 0, 500));
        Timer LogicUpdateThreshold = new Timer(new TimeSpan(0, 0, 0, 0, 500));
        Random rand = new Random();
        Direction CurrentDirection = Direction.None;
        Timer TargetTime = new Timer(new TimeSpan(0, 0, 5));

        private RandomList<int[]> PlacesToGo = new RandomList<int[]>();

        // New
        private bool GoingTowardsSafety = false;
        private readonly Vector2 NullVector2 = new Vector2(-2, -2);
        private BotTarget CurrentTarget;
        private bool SwitchMovements = false;
        public BotActor(Texture2D skin, int profile, Vector2 pos, GameScreenPerformer parentgamescreen, Color markingcolor, int plyeridx, IResources resources,
            IFrameTimeService frameTimeService, MatchSettings matchSettings) :
            base(skin, profile, pos, null, markingcolor, plyeridx, resources, frameTimeService, matchSettings)
        {
            Gamepad = AIInput;
            ParentGameScreen = parentgamescreen;
            _frameTimeService = frameTimeService;
            IsBot = true;

            PlacesToGo.Add(new int[] { 0, 1 });
            PlacesToGo.Add(new int[] { 0, -1 });
            PlacesToGo.Add(new int[] { 1, 0 });
            PlacesToGo.Add(new int[] { -1, 0 });
        }
        public override void Update(Vector2 CurrentCoordinates, Vector2 TilePos, GameScreenState gameScreenState)
        {
            AIInput.PressedAction2 = false;

            DiagonalMovementSwitch.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (DiagonalMovementSwitch.Active)
            {
                SwitchMovements = rand.Next(0, 2) == 1;
            }

            LogicUpdateThreshold.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
            if (LogicUpdateThreshold.Active)
            {
                LogicUpdate(CurrentCoordinates, TilePos, gameScreenState);
                LogicUpdateThreshold.Reset();
            }

            CreateInput();

            base.Update(CurrentCoordinates, TilePos, gameScreenState);

            ClearInput();
        }

        private void LogicUpdate(Vector2 CurrentCoordinates, Vector2 TilePos, GameScreenState gameScreenState)
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
                    AIInput.PressedAction2 = true;

                // Not Safe need to do something
                if (GoingTowardsSafety)
                {
                    // Just needs to move
                    if (CurrentTarget == null || CurrentCoordinates == CurrentTarget.Position)
                        GoingTowardsSafety = false;
                }

                if (!GoingTowardsSafety)
                {
                    GoingTowardsSafety = true;

                    Vector2 PlaceToGo = FindSafePlace(CurrentCoordinates, gameScreenState);

                    if (PlaceToGo == NullVector2)
                    {
                        CurrentTarget = null;
                    }
                    else
                    {
                        CurrentTarget = new BotTarget(PlaceToGo);
                    }
                }
                Attacking = false;
            }
            else
            {
                if (GoingTowardsSafety)
                {
                    GoingTowardsSafety = false;
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
                        Vector2 pos = GameScreenFunctions.InterpretCoordinates(gameScreenState, gameScreenState.Characters[x].Position, true);
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
                    CurrentTarget = null;
                }
                else
                {
                    CurrentTarget = Targets[SavedTarget];

                    if (HasPowerup(PowerupUse.Strategy))
                        AIInput.PressedAction2 = true;
                }
            }

            Attacking = CurrentTarget != null &&
                CurrentTarget.ThisTargetType == BotTarget.TargetType.Character &&
                gameScreenState.Characters[CurrentTarget.PlayerIndex].CurrentState != CharacterState.Dead &&
                gameScreenState.Characters[CurrentTarget.PlayerIndex].CurrentState != CharacterState.Dieing;

            if (Moving)
            {
                MakeMovements(CurrentCoordinates, Attacking, gameScreenState.Tiles);
            }
            else
            {
                CurrentDirection = Direction.None;
            }
        }

        private void ClearInput()
        {
            AIInput.PressedAction = false;
            AIInput.PressedAction2 = false;
            AIInput.PressedBack = false;
            AIInput.PressedDown = false;
            AIInput.PressedLeft = false;
            AIInput.PressedRight = false;
            AIInput.PressedStart = false;
            AIInput.PressedUp = false;
            AIInput.PressingDown = false;
            AIInput.PressingLeft = false;
            AIInput.PressingRight = false;
            AIInput.PressingUp = false;
        }

        private bool CurrentTargetIsGood(List<CharacterActor> characterActors)
        {
            if (CurrentTarget != null && CurrentTarget.ThisTargetType == BotTarget.TargetType.Character)
            {
                if (!(characterActors[CurrentTarget.PlayerIndex].CurrentState == CharacterState.Dead) &&
                    !(characterActors[CurrentTarget.PlayerIndex].CurrentState == CharacterState.Dieing))
                {
                    TargetTime.Update(_frameTimeService.GetLatestFrame().MovementFactorTimeSpan);
                    if (TargetTime.Active)
                        TargetTime.Reset();
                    else
                        return true;
                }
            }
            return false;
        }

        private void MakeMovements(Vector2 CurrentCoordinates, bool Attacking, Tile[,] tiles)
        {

            if (CurrentTarget == null)
            {
                CurrentDirection = Direction.None;
            }
            else
            {
                //CurrentDirection = Direction.None;
                if (CurrentCoordinates.X != CurrentTarget.Position.X)
                {
                    if (CurrentCoordinates.X > CurrentTarget.Position.X && IsSafe(tiles, CurrentCoordinates, -1, 0))
                    {
                        CurrentDirection = Direction.Left;
                    }
                    else if (CurrentCoordinates.X < CurrentTarget.Position.X && IsSafe(tiles, CurrentCoordinates, 1, 0))
                    {
                        CurrentDirection = Direction.Right;
                    }
                }

                if (CurrentCoordinates.Y != CurrentTarget.Position.Y)
                {
                    if (CurrentCoordinates.Y > CurrentTarget.Position.Y - 1)
                    {
                        if (CurrentDirection == Direction.Left && IsSafe(tiles, CurrentCoordinates, -1, -1))
                            CurrentDirection = Direction.UpperLeft;
                        else if (CurrentDirection == Direction.Right && IsSafe(tiles, CurrentCoordinates, 1, -1))
                            CurrentDirection = Direction.UpperRight;
                        else if (IsSafe(tiles, CurrentCoordinates, 0, -1))
                            CurrentDirection = Direction.Up;
                    }
                    else if (CurrentCoordinates.Y < CurrentTarget.Position.Y + 1)
                    {
                        if (CurrentDirection == Direction.Left && IsSafe(tiles, CurrentCoordinates, -1, 1))
                            CurrentDirection = Direction.LowerLeft;
                        else if (CurrentDirection == Direction.Right && IsSafe(tiles, CurrentCoordinates, 1, 1))
                            CurrentDirection = Direction.LowerRight;
                        else if (IsSafe(tiles, CurrentCoordinates, 0, 1))
                            CurrentDirection = Direction.Down;
                    }
                }

                if (Attacking)
                {
                    if (HasAttackingPowerup(CurrentCoordinates))
                        AIInput.PressedAction2 = true;
                }

                if (Attacking && GetDistance(CurrentCoordinates, CurrentTarget.Position) <= 8f)
                { // Right Distance
                    if (CurrentCoordinates.X == CurrentTarget.Position.X ||
                        CurrentCoordinates.Y == CurrentTarget.Position.Y)
                    { // Right Position

                        AIInput.PressedAction = true;
                    }
                }
            }
        }

        private Vector2 FindSafePlace(Vector2 CurrentCoordinates, GameScreenState gameScreenState)
        {
            PlacesToGo.RandomizeList();

            Vector2 SafePlace = Vector2.Zero;

            bool FoundSafePlace = false;

            for (int x = 0; x < PlacesToGo.Count; x++)
            {
                Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + PlacesToGo[x][0], CurrentCoordinates.Y + PlacesToGo[x][1]);
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

                for (int x = 0; x < PlacesToGo.Count; x++)
                {
                    Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + PlacesToGo[x][0], CurrentCoordinates.Y + PlacesToGo[x][1]);
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
                    return NullVector2;
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
                if (GetDistance(CurrentCoordinates, CurrentTarget.Position) <= CurrentPowerup.AttackingRange)
                {
                    if (CurrentPowerup.AttackingInLine)
                    {
                        if (CurrentCoordinates.X == CurrentTarget.Position.X ||
                            CurrentCoordinates.Y == CurrentTarget.Position.Y)
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
            AIInput.PressedLeft = false;
            AIInput.PressedRight = false;
            AIInput.PressedUp = false;
            AIInput.PressedDown = false;

            AIInput.PressingLeft = false;
            AIInput.PressingRight = false;
            AIInput.PressingUp = false;
            AIInput.PressingDown = false;

            if (CurrentDirection == Direction.Left)
                AIInput.PressingLeft = true;
            else if (CurrentDirection == Direction.Right)
                AIInput.PressingRight = true;
            else if (CurrentDirection == Direction.Up)
                AIInput.PressingUp = true;
            else if (CurrentDirection == Direction.Down)
                AIInput.PressingDown = true;
            else if (CurrentDirection == Direction.LowerLeft)
            {
                if (SwitchMovements)
                    AIInput.PressingDown = true;
                else
                    AIInput.PressingLeft = true;
            }
            else if (CurrentDirection == Direction.LowerRight)
            {
                if (SwitchMovements)
                    AIInput.PressingDown = true;
                else
                    AIInput.PressingRight = true;
            }
            else if (CurrentDirection == Direction.UpperLeft)
            {
                if (SwitchMovements)
                    AIInput.PressingUp = true;
                else
                    AIInput.PressingLeft = true;
            }
            else if (CurrentDirection == Direction.UpperRight)
            {
                if (SwitchMovements)
                    AIInput.PressingUp = true;
                else
                    AIInput.PressingRight = true;
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
}
