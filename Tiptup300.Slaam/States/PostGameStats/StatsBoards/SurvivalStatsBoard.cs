using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.Library.Logging;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.PlayerProfiles;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.PostGameStats.StatsBoards;

public class SurvivalStatsBoard : StatsBoard
{
   public List<SurvivalStatsPageListing> SurvivalStatsPage;
   public SurvivalStatsPageListing[] PeopleToCompare;
   public bool AddingNewChar = true;

   private int NewScoreLoc;
   private int RowsToDraw;

   private ILogger _logger;
   public SurvivalStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col, int rowstodraw, ILogger logger, IResources resources, IRenderService renderGraph, StatsScreenState statsScreenState)
       : base(scorekeeper, statsScreenState)
   {
      _logger = logger;

      MainBoard = new Graph(rect, 2, col, resources, renderGraph);
      RowsToDraw = rowstodraw;
   }

   public override void CalculateStats()
   {
      AddingNewChar = ParentScoreCollector != null;
      XNALoadHighScores();

      if (AddingNewChar)
      {
         PeopleToCompare[0] = new SurvivalStatsPageListing(_statsScreenState.Characters[0].GetProfile().Name, _statsScreenState.Characters[0].Kills, _statsScreenState.Characters[0].TimeAlive,
         DateTime.Now.ToString());
      }

      SurvivalStatsPage = new List<SurvivalStatsPageListing>();

      int AmtSelected = 0, CurrentPlace = 1;
      bool[] SelectedAlready = new bool[PeopleToCompare.Length];

      while (AmtSelected < PeopleToCompare.Length)
      {
         TimeSpan highest = TimeSpan.Zero;
         List<int> IndexsSelected = new List<int>();

         for (int x = 0; x < PeopleToCompare.Length; x++)
         {
            if (!SelectedAlready[x])
            {
               if (PeopleToCompare[x].TimeSurvived > highest)
               {
                  IndexsSelected.Clear();
                  IndexsSelected.Add(x);
                  highest = PeopleToCompare[x].TimeSurvived;
               }
               else if (PeopleToCompare[x].TimeSurvived == highest)
               {
                  IndexsSelected.Add(x);
               }
            }
         }
         for (int x = 0; x < IndexsSelected.Count; x++)
         {
            SurvivalStatsPage.Add(new SurvivalStatsPageListing(/*(Places)*/GetRank(CurrentPlace).ToString(), PeopleToCompare[IndexsSelected[x]]));
            if (IndexsSelected[x] == 0)
               NewScoreLoc = SurvivalStatsPage.Count - 1;
            AmtSelected++;
            SelectedAlready[IndexsSelected[x]] = true;
         }
         CurrentPlace++;
      }
      if (AddingNewChar)
         XNASaveHighScores();
      else
         NewScoreLoc = -2;
   }

   public override Graph ConstructGraph(int index)
   {
      MainBoard.Items.Columns.Add("");
      MainBoard.Items.Columns.Add("Rank");
#if !ZUNE
      MainBoard.Items.Columns.Add("Kills");
      MainBoard.Items.Columns.Add("Date Set");
#endif
      MainBoard.Items.Columns.Add("Time");
      for (int x = 0; x < RowsToDraw; x++)
      {
         if (x >= SurvivalStatsPage.Count)
            break;

         GraphItem itm = new GraphItem();
         {
            itm.Details.Add(SurvivalStatsPage[x].Name);
            itm.Details.Add(SurvivalStatsPage[x].Place.ToString());
#if !ZUNE
            itm.Details.Add(SurvivalStatsPage[x].Kills.ToString());
            itm.Details.Add(SurvivalStatsPage[x].DateSet);
#endif
            itm.Details.Add(NormalStatsBoard.TimeSpanToString(SurvivalStatsPage[x].TimeSurvived));
            MainBoard.Items.Add(itm);
         }
      }
      MainBoard.CalculateBlocks();
      if (NewScoreLoc < RowsToDraw && NewScoreLoc != -2)
         MainBoard.SetHighlight(NewScoreLoc);
      return MainBoard;
   }

   public struct SurvivalStatsPageListing
   {
      public string Name;
      public string Place;
      public int Kills;
      public TimeSpan TimeSurvived;
      public string DateSet;

      public SurvivalStatsPageListing(string name, int kills, TimeSpan timesurvived, string dateset)
      {
         Name = name;
         Place = "";
         Kills = kills;
         TimeSurvived = timesurvived;
         DateSet = dateset;
      }

      public SurvivalStatsPageListing(string place, SurvivalStatsPageListing listing)
      {
         Place = place;
         Name = listing.Name;
         Kills = listing.Kills;
         TimeSurvived = listing.TimeSurvived;
         DateSet = listing.DateSet;
      }
   }

   public void XNALoadHighScores()
   {
      XnaContentReader reader = new XnaContentReader(_logger, ServiceLocator.Instance.GetService<ProfileFileVersion>());
      reader.Initialize(DialogStrings._["SurvivalScoresFilename"]);
      reader.IsWrongVersion();

      int ProfileAmt;

      try
      {
         ProfileAmt = reader.ReadInt32();
      }
      catch
      {
         ProfileAmt = 0;
      }

      if (AddingNewChar)
         ProfileAmt++;

      PeopleToCompare = new SurvivalStatsPageListing[ProfileAmt];

      for (int x = AddingNewChar ? 1 : 0; x < ProfileAmt; x++)
      {
         string Name = reader.ReadString();
         int Kills = reader.ReadInt32();
         TimeSpan TimeSurvived = new TimeSpan(0, 0, 0, 0, reader.ReadInt32());
         string DateSet = reader.ReadString();
         PeopleToCompare[x] = new SurvivalStatsPageListing(Name, Kills, TimeSurvived, DateSet);
      }

      reader.Close();
   }

   public void XNASaveHighScores()
   {
      XnaContentWriter writer = new XnaContentWriter(ServiceLocator.Instance.GetService<ProfileFileVersion>());
      writer.Initialize(DialogStrings._["SurvivalScoresFilename"]);
      writer.Write(PeopleToCompare.Length);

      for (int x = 0; x < PeopleToCompare.Length; x++)
      {
         writer.Write(PeopleToCompare[x].Name);
         writer.Write(PeopleToCompare[x].Kills);
         writer.Write((int)Math.Round(PeopleToCompare[x].TimeSurvived.TotalMilliseconds, 0));
         writer.Write(PeopleToCompare[x].DateSet);
      }

      writer.Close();
   }

   private string GetRank(int x)
   {
      if (x == 1)
         return "1st";
      else if (x == 2)
         return "2nd";
      else if (x == 3)
         return "3rd";
      else
         return x + "th";
   }
}
