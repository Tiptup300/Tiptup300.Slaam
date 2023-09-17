using Microsoft.Xna.Framework;
using SlaamMono.Gameplay;
using SlaamMono.Gameplay.Statistics;
using SlaamMono.Library.Graphing;
using SlaamMono.Library.ResourceManagement;
using System;
using System.Collections.Generic;
using Tiptup300.Slaam.Library.Rendering;

namespace SlaamMono.States.PostGameStats.StatsBoards;

class NormalStatsBoard : StatsBoard
 {
     public NormalPlayerStatsPageListing[] NormalStatsPage;

     public NormalStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col, IResources resources, IRenderService renderGraph, StatsScreenState statsScreenState)
         : base(scorekeeper, statsScreenState)
     {
         NormalStatsPage = new NormalPlayerStatsPageListing[statsScreenState.Characters.Count];
         MainBoard = new Graph(rect, 2, col, resources, renderGraph);
     }

     public override void CalculateStats()
     {

         TimeSpan[] TotalTime = new TimeSpan[_statsScreenState.Characters.Count];
         for (int x = 0; x < _statsScreenState.Characters.Count; x++)
         {
             if (_statsScreenState.Characters[x].Lives > 0)
                 TotalTime[x] = _statsScreenState.Characters[x].TimeAlive + new TimeSpan(0, 5, 0);
             else
                 TotalTime[x] = _statsScreenState.Characters[x].TimeAlive;
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

                 if (_statsScreenState.Characters[x].IsBot)
                     itm.Details.Add("*" + _statsScreenState.Characters[x].GetProfile().Name + "*");
                 else
                     itm.Details.Add(_statsScreenState.Characters[x].GetProfile().Name);

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
