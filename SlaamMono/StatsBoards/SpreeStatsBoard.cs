using Microsoft.Xna.Framework;
using SlaamMono.SubClasses;
using System.Collections.Generic;

namespace SlaamMono.StatsBoards
{
    class SpreeStatsBoard : StatsBoard
    {
        public SpreePlayerStatsPageListing[] SpreeStatsPage;

        public SpreeStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col)
            : base(scorekeeper)
        {
            SpreeStatsPage = new SpreePlayerStatsPageListing[scorekeeper.ParentGameScreen.Characters.Count];
            MainBoard = new Graph(rect, 2, col);
        }

        public override void CalculateStats()
        {
            int[] TotalScore = new int[ParentScoreCollector.ParentGameScreen.Characters.Count];
            for (int x = 0; x < ParentScoreCollector.ParentGameScreen.Characters.Count; x++)
            {
                for (int y = 0; y < ParentScoreCollector.ParentGameScreen.Characters.Count; y++)
                {
                    if (x != y)
                        TotalScore[x] += ParentScoreCollector.Kills[x][y];
                    else
                        TotalScore[x] -= ParentScoreCollector.Kills[x][y];
                }
            }

            int AmtSelected = 0, CurrentPlace = 1;
            bool[] SelectedAlready = new bool[TotalScore.Length];

            while (AmtSelected < TotalScore.Length)
            {
                int highest = -1;
                List<int> IndexsSelected = new List<int>();

                for (int x = 0; x < TotalScore.Length; x++)
                {
                    if (!SelectedAlready[x])
                    {
                        if (TotalScore[x] > highest)
                        {
                            IndexsSelected.Clear();
                            IndexsSelected.Add(x);
                            highest = TotalScore[x];
                        }
                        else if (TotalScore[x] == highest)
                        {
                            IndexsSelected.Add(x);
                        }
                    }
                }
                for (int x = 0; x < IndexsSelected.Count; x++)
                {
                    SpreeStatsPage[IndexsSelected[x]] = new SpreePlayerStatsPageListing(((Places)CurrentPlace).ToString(), ParentScoreCollector.BestSprees[IndexsSelected[x]], TotalScore[IndexsSelected[x]]);
                    AmtSelected++;
                    SelectedAlready[IndexsSelected[x]] = true;
                }
                CurrentPlace++;
            }
        }

        public override Graph ConstructGraph(int index)
        {
            MainBoard.Items.Columns.Add("");
            MainBoard.Items.Columns.Add("Place");
            MainBoard.Items.Columns.Add("Best Spree");
            MainBoard.Items.Columns.Add("Score");

            for (int x = 0; x < SpreeStatsPage.Length; x++)
            {

                GraphItem itm = new GraphItem();
                {

                    if (ParentScoreCollector.ParentGameScreen.Characters[x].IsBot)
                        itm.Details.Add("*" + ParentScoreCollector.ParentGameScreen.Characters[x].GetProfile().Name + "*");
                    else
                        itm.Details.Add(ParentScoreCollector.ParentGameScreen.Characters[x].GetProfile().Name);

                    itm.Details.Add(SpreeStatsPage[x].Place.ToString());
                    itm.Details.Add(SpreeStatsPage[x].BestSpree.ToString());
                    itm.Details.Add(SpreeStatsPage[x].Score.ToString());

                    MainBoard.Items.Add(itm);
                }
            }
            MainBoard.CalculateBlocks();
            return MainBoard;
        }

        public struct SpreePlayerStatsPageListing
        {
            public string Place;
            public int BestSpree;
            public int Score;

            public SpreePlayerStatsPageListing(string place, int bestspree, int score)
            {
                Place = place;
                BestSpree = bestspree;
                Score = score;
            }

        }
    }


}
