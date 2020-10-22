using System;
using System.Collections.Generic;
using System.Text;

namespace Slaam
{
    public class MatchScoreCollection
    {
        #region Variables

        public GameScreen ParentGameScreen;
        public int[][] Kills;
        public int[] BestSprees;
        private int[] Sprees;


        #endregion

        public MatchScoreCollection(GameScreen parentgamecreen)
        {
            ParentGameScreen = parentgamecreen;

            Kills = new int[ParentGameScreen.Characters.Count][];
            BestSprees = new int[ParentGameScreen.Characters.Count];
            Sprees = new int[ParentGameScreen.Characters.Count];
            for (int x = 0; x < Kills.Length; x++)
            {
                Kills[x] = new int[ParentGameScreen.Characters.Count];
            }
        }

        public void CalcTotals()
        {
            if (ParentGameScreen.ThisGameType != GameType.Survival)
            {
                for (int x = 0; x < ParentGameScreen.Characters.Count; x++)
                    ResetSpree(x);
            }

        }

        /// <summary>
        /// Update the kill tables for final scoring.
        /// </summary>
        /// <param name="Killer">Index of who killed.</param>
        /// <param name="Killee">Index of who was killed.</param>
        public void ReportKilling(int Killer, int Killee)
        {
            if (ParentGameScreen.ThisGameType == GameType.Survival)
            {
                if (Killer == 0)
                    Kills[0][0]++;
            }
            else
            {
                if (Killer != -2 && Killer < ParentGameScreen.Characters.Count)
                {
                    Kills[Killer][Killee]++;
                    Sprees[Killer]++;
                }
                else
                    Kills[Killee][Killee]++;

                ResetSpree(Killee);
            }

        }

        private void ResetSpree(int PlayerIndex)
        {
            if(Sprees[PlayerIndex] > BestSprees[PlayerIndex])
                BestSprees[PlayerIndex] = Sprees[PlayerIndex];

            Sprees[PlayerIndex] = 0;
        }
    }

    #region Places Enum

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

        #endregion

    public enum GameType
    {
        Classic,     // Survival
        Spree,      // Deathmatch
        TimedSpree, // Timed Deathmatch
        Survival,  // Fighting Polygon Team!
    }
}
