using Microsoft.Xna.Framework;
using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.Rendering;
using SlaamMono.Library.ResourceManagement;
using System.Collections.Generic;

namespace SlaamMono.StatsBoards
{
    public class KillsStatsBoard : StatsBoard
    {
        public List<KillsPageListing> KillsPage = new List<KillsPageListing>();

        public KillsStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col, IResources resourcesManager, IRenderGraph renderGraphManager, StatsScreenState statsScreenState)
            : base(scorekeeper, statsScreenState)
        {
            MainBoard = new Graph(rect, 2, col, resourcesManager, renderGraphManager);
        }

        public override void CalculateStats()
        {
            int[] TotalKills = new int[_statsScreenState.Characters.Count];
            for (int x = 0; x < _statsScreenState.Characters.Count; x++)
            {

                for (int y = 0; y < _statsScreenState.Characters.Count; y++)
                {
                    if (x != y)
                        TotalKills[x] += ParentScoreCollector.Kills[x][y];
                }

            }

            int[] TotalDeaths = new int[_statsScreenState.Characters.Count];
            for (int x = 0; x < _statsScreenState.Characters.Count; x++)
            {

                for (int y = 0; y < _statsScreenState.Characters.Count; y++)
                {
                    TotalDeaths[x] += ParentScoreCollector.Kills[y][x];
                }

            }

            int[] TotalSuicides = new int[_statsScreenState.Characters.Count];
            for (int x = 0; x < _statsScreenState.Characters.Count; x++)
            {
                TotalSuicides[x] = ParentScoreCollector.Kills[x][x];
            }

            for (int x = 0; x < _statsScreenState.Characters.Count; x++)
            {
                KillsPage.Add(new KillsPageListing(TotalKills[x], TotalDeaths[x], TotalSuicides[x]));
            }
        }

        public override Graph ConstructGraph(int index)
        {
            MainBoard.Items.Columns.Add("");
            MainBoard.Items.Columns.Add("Kills");
            MainBoard.Items.Columns.Add("Deaths");
            MainBoard.Items.Columns.Add("Suicides");

            for (int x = 0; x < KillsPage.Count; x++)
            {
                GraphItem itm = new GraphItem();
                {

                    if (_statsScreenState.Characters[x].IsBot)
                        itm.Details.Add("*" + _statsScreenState.Characters[x].GetProfile().Name + "*");
                    else
                        itm.Details.Add(_statsScreenState.Characters[x].GetProfile().Name);

                    itm.Add(true, KillsPage[x].Kills.ToString(), KillsPage[x].Deaths.ToString(), KillsPage[x].Suicides.ToString());

                    MainBoard.Items.Add(itm);
                }
            }
            MainBoard.CalculateBlocks();
            return MainBoard;
        }
        public struct KillsPageListing
        {
            public int Kills;
            public int Deaths;
            public int Suicides;

            public KillsPageListing(int kills, int deaths, int suicides)
            {
                Kills = kills;
                Deaths = deaths;
                Suicides = suicides;
            }

        }
    }
}
