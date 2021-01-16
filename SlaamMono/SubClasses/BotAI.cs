using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{
    public class BotPlayer : Character
    {
        #region Variables

        GameScreen ParentGameScreen;
        InputDevice AIInput = new InputDevice(InputDeviceType.Other, ExtendedPlayerIndex.Eight, -1);
        Timer DiagonalMovementSwitch = new Timer(new TimeSpan(0, 0, 0,0,500));
        Timer LogicUpdateThreshold = new Timer(new TimeSpan(0, 0, 0,0,500));
        Random rand = new Random();
        Direction CurrentDirection = Direction.None;
        Timer TargetTime = new Timer(new TimeSpan(0, 0, 5));

        private RandomList<int[]> PlacesToGo = new RandomList<int[]>();

        // New
        private bool GoingTowardsSafety = false;
        private readonly Vector2 NullVector2 = new Vector2(-2, -2);
        private Target CurrentTarget;
        private bool SwitchMovements = false;
        #endregion

        #region Constructor

        public BotPlayer(Texture2D skin, int profile, Vector2 pos, GameScreen parentgamescreen, Color markingcolor, int plyeridx):
            base(skin,profile,pos,null,markingcolor,plyeridx)
        {
            Gamepad = AIInput;
            ParentGameScreen = parentgamescreen;
            base.IsBot = true;



            PlacesToGo.Add(new int[] { 0, 1 });
            PlacesToGo.Add(new int[] { 0, -1 });
            PlacesToGo.Add(new int[] { 1, 0 });
            PlacesToGo.Add(new int[] { -1, 0 });
        }

        #endregion


        public override void Update(Tile[,] tiles, Vector2 CurrentCoordinates, Vector2 TilePos)
        {
            AIInput.PressedAction2 = false;

            DiagonalMovementSwitch.Update(FPSManager.MovementFactorTimeSpan);
            if (DiagonalMovementSwitch.Active)
            {
                SwitchMovements = rand.Next(0, 2) == 1;
            }

            LogicUpdateThreshold.Update(FPSManager.MovementFactorTimeSpan);
            if (LogicUpdateThreshold.Active)
            {
                LogicUpdate(tiles, CurrentCoordinates, TilePos);
                LogicUpdateThreshold.Reset();
            }
            


            CreateInput();

            base.Update(tiles, CurrentCoordinates, TilePos);

            ClearInput();
        }

        private void LogicUpdate(Tile[,] tiles, Vector2 CurrentCoordinates, Vector2 TilePos)
        {
            Tile CurrentTile = tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y];
            bool Moving = true, Attacking = false, InDanger = (CurrentTile.CurrentTileCondition != Tile.TileCondition.Normal && CurrentTile.CurrentTileCondition != Tile.TileCondition.RespawnPoint);


            if (base.CurrentState == CharacterState.Dead || base.CurrentState == CharacterState.Dieing)
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

                    Vector2 PlaceToGo = FindSafePlace(CurrentCoordinates);

                    if (PlaceToGo == NullVector2)
                    {
                        CurrentTarget = null;
                    }
                    else
                    {
                        CurrentTarget = new Target(PlaceToGo);
                    }
                }
                Attacking = false;
            }
            else /* if(!CurrentTargetIsGood()) */
            {
                if (GoingTowardsSafety)
                {
                    GoingTowardsSafety = false;
                }
                List<Target> Targets = new List<Target>();

                for (int x = 0; x < ParentGameScreen.Characters.Count; x++)
                {
                    if (x != base.PlayerIndex &&
                         ParentGameScreen.Characters[x] != null &&
                         ParentGameScreen.Characters[x].CurrentState != CharacterState.Dead &&
                         ParentGameScreen.Characters[x].CurrentState != CharacterState.Dieing &&
                         /*( ParentGameScreen.Characters[x].CurrentTile != null && ParentGameScreen.Characters[x].CurrentTile.CurrentTileCondition != Tile.TileCondition.RespawnPoint ) && */
                         ParentGameScreen.Characters[x].MarkingColor != base.MarkingColor)
                    {
                        Vector2 pos = ParentGameScreen.InterpretCoordinates(ParentGameScreen.Characters[x].Position, true);
                        if (pos != CurrentCoordinates)
                            Targets.Add(new Target(x, pos, GetDistance(CurrentCoordinates, pos)));

                    }
                }

                for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
                {
                    for (int y = 0; y < GameGlobals.BOARD_HEIGHT; y++)
                    {
                        if (tiles[x, y].CurrentPowerupType != PowerupType.None)
                        {
                            float distance = GetDistance(new Vector2(x, y), CurrentCoordinates);
                            Targets.Add(new Target(new Vector2(x, y), distance));
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

            Attacking = (CurrentTarget != null &&
    CurrentTarget.ThisTargetType == Target.TargetType.Character &&
    ParentGameScreen.Characters[CurrentTarget.PlayerIndex].CurrentState != CharacterState.Dead &&
    ParentGameScreen.Characters[CurrentTarget.PlayerIndex].CurrentState != CharacterState.Dieing);

            if (Moving)
            {
                MakeMovements(CurrentCoordinates, Attacking);
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

        private bool CurrentTargetIsGood()
        {
            if (CurrentTarget != null && CurrentTarget.ThisTargetType == Target.TargetType.Character)
            {
                if (!(ParentGameScreen.Characters[CurrentTarget.PlayerIndex].CurrentState == CharacterState.Dead) &&
                    !(ParentGameScreen.Characters[CurrentTarget.PlayerIndex].CurrentState == CharacterState.Dieing))
                {
                    TargetTime.Update(FPSManager.MovementFactorTimeSpan);
                    if (TargetTime.Active)
                        TargetTime.Reset();
                    else
                        return true;
                }
            }
            return false;
        }

        private void MakeMovements(Vector2 CurrentCoordinates, bool Attacking)
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
                    if (CurrentCoordinates.X > CurrentTarget.Position.X && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, -1, 0))
                    {
                        CurrentDirection = Direction.Left;
                    }
                    else if (CurrentCoordinates.X < CurrentTarget.Position.X && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, 1, 0))
                    {
                        CurrentDirection = Direction.Right;
                    }
                }

                if (CurrentCoordinates.Y != CurrentTarget.Position.Y)
                {
                    if (CurrentCoordinates.Y > CurrentTarget.Position.Y - 1)
                    {
                        if (CurrentDirection == Direction.Left && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, -1, -1))
                            CurrentDirection = Direction.UpperLeft;
                        else if (CurrentDirection == Direction.Right && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, 1, -1))
                            CurrentDirection = Direction.UpperRight;
                        else if (IsSafe(ParentGameScreen.tiles, CurrentCoordinates, 0, -1))
                            CurrentDirection = Direction.Up;
                    }
                    else if (CurrentCoordinates.Y < CurrentTarget.Position.Y + 1)
                    {
                        if (CurrentDirection == Direction.Left && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, -1, 1))
                            CurrentDirection = Direction.LowerLeft;
                        else if (CurrentDirection == Direction.Right && IsSafe(ParentGameScreen.tiles, CurrentCoordinates, 1, 1))
                            CurrentDirection = Direction.LowerRight;
                        else if (IsSafe(ParentGameScreen.tiles, CurrentCoordinates, 0, 1))
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

        private Vector2 FindSafePlace(Vector2 CurrentCoordinates)
        {
            PlacesToGo.RandomizeList();

            Vector2 SafePlace = Vector2.Zero;

            bool FoundSafePlace = false;

            for (int x = 0; x < PlacesToGo.Count; x++)
            {
                Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + PlacesToGo[x][0], CurrentCoordinates.Y + PlacesToGo[x][1]);
                if (IsSafeAndClear(CurrentTileLocation))
                {
                    SafePlace = new Vector2(CurrentTileLocation.X,CurrentTileLocation.Y);
                    FoundSafePlace = true;
                }

            }

            if (FoundSafePlace)
            {
                return SafePlace;
            }
            else 
            {
                float Highest = ParentGameScreen.tiles[(int)CurrentCoordinates.X, (int)CurrentCoordinates.Y].TimeTillClearing;

                for (int x = 0; x < PlacesToGo.Count; x++)
                {
                    Vector2 CurrentTileLocation = new Vector2(CurrentCoordinates.X + PlacesToGo[x][0], CurrentCoordinates.Y + PlacesToGo[x][1]);
                    if (IsClear(CurrentTileLocation))
                    {
                        float temp = ParentGameScreen.tiles[(int)CurrentTileLocation.X, (int)CurrentTileLocation.Y].TimeTillClearing;

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

        #region Movement Methods

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

        #endregion

        #region Positioning Methods

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
            return base.IsSafe(new Vector2(Coords.X + x, Coords.Y + y));
        }

        #endregion

        #region Enums

        private enum BotStatus
        {
            GoingTowardsEnemy,
            GoingTowardsSafety,
            Roaming,
        }

        #endregion

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
