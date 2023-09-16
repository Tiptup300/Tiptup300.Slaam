using Microsoft.Xna.Framework;
using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using System.Collections.Generic;

namespace SlaamMono.States.PostGameStats.StatsBoards
{
    class SpreeStatsBoard : StatsBoard
    {
        public SpreePlayerStatsPageListing[] SpreeStatsPage;

        public SpreeStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col, IResources resources, IRenderService renderGraph, StatsScreenState statsScreenState)
            : base(scorekeeper, statsScreenState)
        {
            SpreeStatsPage = new SpreePlayerStatsPageListing[_statsScreenState.Characters.Count];
            MainBoard = new Graph(rect, 2, col, resources, renderGraph);
        }

        public override void CalculateStats()
        {
            int[] TotalScore = new int[_statsScreenState.Characters.Count];
            for (int x = 0; x < _statsScreenState.Characters.Count; x++)
            {
                for (int y = 0; y < _statsScreenState.Characters.Count; y++)
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

                    if (_statsScreenState.Characters[x].IsBot)
                        itm.Details.Add("*" + _statsScreenState.Characters[x].GetProfile().Name + "*");
                    else
                        itm.Details.Add(_statsScreenState.Characters[x].GetProfile().Name);

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
