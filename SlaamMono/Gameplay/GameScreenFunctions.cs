using Microsoft.Xna.Framework;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Library;
using SlaamMono.x_;
using System;

namespace SlaamMono.Gameplay
{
    public static class GameScreenFunctions
    {
        private static void markBoardOutline()
        {
            for (int x = 0; x < GameGlobals.BOARD_WIDTH; x++)
            {
                // TODO FIX LOGIC!
                /*tiles[x, 0 + Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[x, 15 - Boardsize].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[0 + Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);
                tiles[15 - Boardsize, x].MarkTile(Color.Black, ShortenTime, true, -2);*/
            }
        }

        public static void ShortenBoard(GameScreenState gameScreenState)
        {
            TimeSpan ShortenTime = new TimeSpan(0, 0, 0, 2);
            if (gameScreenState.BoardSize < 6)
            {
                markBoardOutline();
                gameScreenState.BoardSize++;
            }
            gameScreenState.StepsRemaining--;
            if (gameScreenState.StepsRemaining == 0)
            {
                gameScreenState.CurrentGameStatus = GameStatus.Over;
                gameScreenState.ReadySetGoPart = 3;
                gameScreenState.ReadySetGoThrottle.Update(FrameRateDirector.Instance.GetLatestFrame().MovementFactorTimeSpan);
            }
        }
        public static void RespawnCharacter(GameScreenState gameScreenState, int characterIndex)
        {
            int newx = gameScreenState.Rand.Next(0, GameGlobals.BOARD_WIDTH);
            int newy = gameScreenState.Rand.Next(0, GameGlobals.BOARD_HEIGHT);

            while (gameScreenState.Tiles[newx, newy].Dead || gameScreenState.Tiles[newx, newy].CurrentTileCondition == TileCondition.RespawnPoint)
            {
                newx = gameScreenState.Rand.Next(0, GameGlobals.BOARD_WIDTH);
                newy = gameScreenState.Rand.Next(0, GameGlobals.BOARD_HEIGHT);
            }
            Vector2 newCharPos = InterpretCoordinates(gameScreenState, new Vector2(newx, newy), false);
            gameScreenState.Characters[characterIndex].Respawn(new Vector2(newCharPos.X + GameGlobals.TILE_SIZE / 2f, newCharPos.Y + GameGlobals.TILE_SIZE / 2f), new Vector2(newx, newy), gameScreenState.Tiles);
        }
        public static Vector2 InterpretCoordinates(GameScreenState gameScreenState, Vector2 position, bool flip)
        {
            if (!flip)
            {
                return new Vector2(gameScreenState.Boardpos.X + position.X * GameGlobals.TILE_SIZE, gameScreenState.Boardpos.Y + position.Y * GameGlobals.TILE_SIZE);
            }
            else
            {

                int X1 = (int)((position.X - gameScreenState.Boardpos.X) % GameGlobals.TILE_SIZE);
                int Y1 = (int)((position.Y - gameScreenState.Boardpos.Y) % GameGlobals.TILE_SIZE);
                int X = (int)((position.X - gameScreenState.Boardpos.X - X1) / GameGlobals.TILE_SIZE);
                int Y = (int)((position.Y - gameScreenState.Boardpos.Y - Y1) / GameGlobals.TILE_SIZE);

                if (position.X < gameScreenState.Boardpos.X)
                    X = -1;
                if (position.Y < gameScreenState.Boardpos.Y)
                    Y = -1;

                return new Vector2(X, Y);
            }
        }

        // to remove
        public static void ReportKilling(int Killer, int Killee, GameScreenState gameScreenState)
        {
            if (gameScreenState.GameType == GameType.Survival)
            {
                survival_ReportKilling(Killer, Killee, gameScreenState);
            }
            else
            {
                if (gameScreenState.Characters[Killee].Lives == 0 && gameScreenState.GameType == GameType.Classic)
                {
                    ShortenBoard(gameScreenState);
                }

                if (Killer != -2 && Killer < gameScreenState.Characters.Count)
                {
                    gameScreenState.Characters[Killer].Kills++;
                }
                gameScreenState.ScoreKeeper.ReportKilling(Killer, Killee, gameScreenState);

                if (gameScreenState.GameType == GameType.Spree && Killer != -2)
                {
                    if (gameScreenState.Characters[Killer].Kills > gameScreenState.SpreeHighestKillCount)
                    {
                        gameScreenState.SpreeCurrentStep += gameScreenState.Characters[Killer].Kills - gameScreenState.SpreeHighestKillCount;
                        gameScreenState.SpreeHighestKillCount = gameScreenState.Characters[Killer].Kills;

                        if (gameScreenState.SpreeCurrentStep >= gameScreenState.SpreeStepSize)
                        {
                            gameScreenState.SpreeCurrentStep -= gameScreenState.SpreeStepSize;
                            if (gameScreenState.Characters[Killer].Kills < gameScreenState.KillsToWin && gameScreenState.StepsRemaining == 1)
                            {
                                // WHY IS THIS HAPPENING!?!??!?!
                            }
                            else
                            {
                                ShortenBoard(gameScreenState);
                                int TimesShortened = 100 - gameScreenState.StepsRemaining;
                            }
                        }

                        if (gameScreenState.Characters[Killer].Kills == gameScreenState.KillsToWin)
                        {
                            gameScreenState.StepsRemaining = 1;
                            ShortenBoard(gameScreenState);
                        }
                    }
                }
            }
        }

        public static void survival_ReportKilling(int Killer, int Killee, GameScreenState gameScreenState)
        {
            if (Killer == 0)
            {
                gameScreenState.Characters[Killer].Kills++;
            }

            if (Killee == 0)
            {
                gameScreenState.CurrentGameStatus = GameStatus.Over;
                gameScreenState.ReadySetGoPart = 3;
                gameScreenState.ReadySetGoThrottle.Update(FrameRateDirector.Instance.GetLatestFrame().MovementFactorTimeSpan);
            }
        }
    }
}
