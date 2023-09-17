using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.Library.Rendering;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.PostGameStats.StatsBoards;

class PvPStatsBoard : StatsBoard
{
   public List<PvPPageListing> PvPPage = new List<PvPPageListing>();

   public PvPStatsBoard(MatchScoreCollection scorekeeper, Rectangle rect, Color col, IResources resources, IRenderService renderGraph, StatsScreenState statsScreenState)
       : base(scorekeeper, statsScreenState)
   {
      MainBoard = new Graph(rect, 2, col, resources, renderGraph);
   }

   public override void CalculateStats()
   {
      for (int z = 0; z < _statsScreenState.Characters.Count; z++)
      {
         List<SubPvPPageListing> templist = new List<SubPvPPageListing>();

         for (int y = 0; y < _statsScreenState.Characters.Count; y++)
         {
            templist.Add(new SubPvPPageListing(ParentScoreCollector.Kills[z][y], ParentScoreCollector.Kills[y][z]));
         }

         PvPPage.Add(new PvPPageListing(templist));
      }
   }

   public override Graph ConstructGraph(int index)
   {
      MainBoard.Items.Columns.Clear();
      MainBoard.Items.Columns.Add("");
      MainBoard.Items.Columns.Add("Killed");
      MainBoard.Items.Columns.Add("Killed By");

      MainBoard.Items.Clear();
      for (int x = 0; x < PvPPage[index].Lists.Count; x++)
      {
         GraphItem itm = new GraphItem();
         {

            if (_statsScreenState.Characters[x].IsBot)
               itm.Details.Add("*" + _statsScreenState.Characters[x].GetProfile().Name + "*");
            else
               itm.Details.Add(_statsScreenState.Characters[x].GetProfile().Name);

            itm.Details.Add(PvPPage[index].Lists[x].Killed.ToString());
            itm.Details.Add(PvPPage[index].Lists[x].KilledBy.ToString());

            if (index == x)
               itm.Highlight = true;

            MainBoard.Items.Add(itm);
         }
      }
      MainBoard.CalculateBlocks();
      return MainBoard;
   }

   public struct PvPPageListing
   {
      public List<SubPvPPageListing> Lists;

      public PvPPageListing(List<SubPvPPageListing> lists)
      {
         Lists = lists;
      }

   }

   public struct SubPvPPageListing
   {
      public int Killed;
      public int KilledBy;

      public SubPvPPageListing(int killed, int killedby)
      {
         Killed = killed;
         KilledBy = killedby;
      }

   }
}
