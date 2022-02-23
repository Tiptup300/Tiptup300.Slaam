namespace SlaamMono.Gameplay
{
    public class MatchScoreCollection
    {
        public GameScreen ParentGameScreen;
        private readonly GameType _gameType;
        public int[][] Kills;
        public int[] BestSprees;
        private int[] Sprees;



        public MatchScoreCollection(GameScreen parentgamecreen, GameType gameType, GameScreenState gameScreenState)
        {
            ParentGameScreen = parentgamecreen;
            _gameType = gameType;
            Kills = new int[gameScreenState.Characters.Count][];
            BestSprees = new int[gameScreenState.Characters.Count];
            Sprees = new int[gameScreenState.Characters.Count];
            for (int x = 0; x < Kills.Length; x++)
            {
                Kills[x] = new int[gameScreenState.Characters.Count];
            }
        }

        public void CalcTotals(GameScreenState gameScreenState)
        {
            if (_gameType != GameType.Survival)
            {
                for (int x = 0; x < gameScreenState.Characters.Count; x++)
                {
                    ResetSpree(x);
                }
            }

        }

        /// <summary>
        /// Update the kill tables for final scoring.
        /// </summary>
        /// <param name="Killer">Index of who killed.</param>
        /// <param name="Killee">Index of who was killed.</param>
        public void ReportKilling(int Killer, int Killee, GameScreenState gameScreenState)
        {
            if (_gameType == GameType.Survival)
            {
                if (Killer == 0)
                {
                    Kills[0][0]++;
                }
            }
            else
            {
                if (Killer != -2 && Killer < gameScreenState.Characters.Count)
                {
                    Kills[Killer][Killee]++;
                    Sprees[Killer]++;
                }
                else
                {
                    Kills[Killee][Killee]++;
                }

                ResetSpree(Killee);
            }

        }

        private void ResetSpree(int PlayerIndex)
        {
            if (Sprees[PlayerIndex] > BestSprees[PlayerIndex])
            {
                BestSprees[PlayerIndex] = Sprees[PlayerIndex];
            }

            Sprees[PlayerIndex] = 0;
        }
    }

    public enum Places
    {
        Loser = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5,
        Sixth = 6,
        Seventh = 7,
        Eighth = 8,
    }

    public enum GameType
    {
        Classic,     // Survival
        Spree,      // Deathmatch
        TimedSpree, // Timed Deathmatch
        Survival,  // Fighting Polygon Team!
    }
}
