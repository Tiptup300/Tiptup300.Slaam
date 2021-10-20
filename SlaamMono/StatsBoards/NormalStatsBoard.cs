using Microsoft.Xna.Framework;
using SlaamMono.SubClasses;
using System;
using System.Collections.Generic;

namespace SlaamMono.StatsBoards
{
    class NormalStatsBoard : StatsBoard
    {
        public NormalPlayerStatsPageListing[] NormalStatsPage;

        public NormalStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col)
            : base(scorekeeper)
        {
            NormalStatsPage = new NormalPlayerStatsPageListing[scorekeeper.ParentGameScreen.Characters.Count];
            MainBoard = new Graph(rect, 2, col);
        }

        public override void CalculateStats()
        {

            TimeSpan[] TotalTime = new TimeSpan[ParentScoreCollector.ParentGameScreen.Characters.Count];
            for (int x = 0; x < ParentScoreCollector.ParentGameScreen.Characters.Count; x++)
            {
                if (ParentScoreCollector.ParentGameScreen.Characters[x].Lives > 0)
                    TotalTime[x] = ParentScoreCollector.ParentGameScreen.Characters[x].TimeAlive + new TimeSpan(0, 5, 0);
                else
                    TotalTime[x] = ParentScoreCollector.ParentGameScreen.Characters[x].TimeAlive;
            }

            int AmtSelected = 0, CurrentPlace = 1;
            bool[] SelectedAlready = new bool[TotalTime.Length];

            while (AmtSelected < TotalTime.Length)
            {
                TimeSpan highest = TimeSpan.Zero;
                List<int> IndexsSelected = new List<int>();

                for (int x = 0; x < TotalTime.Length; x++)
                {
                    if (!SelectedAlready[x])
                    {
                        if (TotalTime[x] > highest)
                        {
                            IndexsSelected.Clear();
                            IndexsSelected.Add(x);
                            highest = TotalTime[x];
                        }
                        else if (TotalTime[x] == highest)
                        {
                            IndexsSelected.Add(x);
                        }
                    }
                }
                for (int x = 0; x < IndexsSelected.Count; x++)
                {
                    NormalStatsPage[IndexsSelected[x]] = new NormalPlayerStatsPageListing(((Places)CurrentPlace).ToString(), ParentScoreCollector.BestSprees[IndexsSelected[x]], TotalTime[IndexsSelected[x]]);
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
            MainBoard.Items.Columns.Add("Time Survived");

            for (int x = 0; x < NormalStatsPage.Length; x++)
            {

                GraphItem itm = new GraphItem();
                {

                    if (ParentScoreCollector.ParentGameScreen.Characters[x].IsBot)
                        itm.Details.Add("*" + ParentScoreCollector.ParentGameScreen.Characters[x].GetProfile().Name + "*");
                    else
                        itm.Details.Add(ParentScoreCollector.ParentGameScreen.Characters[x].GetProfile().Name);

                    itm.Details.Add(NormalStatsPage[x].Place.ToString());
                    itm.Details.Add(NormalStatsPage[x].BestSpree.ToString());

                    if (NormalStatsPage[x].Place.ToString() == "First")
                        itm.Details.Add("---");
                    else
                        itm.Details.Add(TimeSpanToString(NormalStatsPage[x].Score));

                    MainBoard.Items.Add(itm);
                }
            }
            MainBoard.CalculateBlocks();
            return MainBoard;
        }

        public static string TimeSpanToString(TimeSpan timespan)
        {
            string hour, min, sec;

            if (timespan.Hours > 1)
                hour = timespan.Hours + " Hours ";
            else if (timespan.Hours == 1)
                hour = timespan.Hours + " Hour";
            else
                hour = "";

            if (timespan.Minutes > 1)
                min = timespan.Minutes + " Mins ";
            else if (timespan.Minutes == 1)
                min = timespan.Minutes + " Min ";
            else
                min = "";

            if (timespan.Seconds > 1)
                sec = timespan.Seconds + " Secs";
            else if (timespan.Seconds == 1)
                sec = timespan.Seconds + " Sec";
            else
                sec = "";

            if ((hour + min + sec).Trim() == "")
                return "0 Secs";

            return hour + min + sec;
        }

        public struct NormalPlayerStatsPageListing
        {
            public string Place;
            public int BestSpree;
            public TimeSpan Score;

            public NormalPlayerStatsPageListing(string place, int bestspree, TimeSpan score)
            {
                Place = place;
                BestSpree = bestspree;
                Score = score;
            }

        }
    }
}
