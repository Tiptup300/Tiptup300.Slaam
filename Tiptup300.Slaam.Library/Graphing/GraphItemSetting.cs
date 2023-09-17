using System.Collections.Generic;
using Tiptup300.Slaam.Library.Widgets;

namespace Tiptup300.Slaam.Library.Graphing;

public class GraphItemSetting : GraphItem
{
   public List<string> Options = new List<string>();
   public IntRange OptionChoice;

   public GraphItemSetting(int val, params string[] str)
       : base()
   {
      Details.Add(str[0]);
      for (int x = 1; x < str.Length; x++)
      {
         Options.Add(str[x]);
      }
      OptionChoice = new IntRange(val, 0, str.Length - 2);
      Details.Add(str[val + 1]);
   }

   public void ChangeValue(bool Adding)
   {
      if (Adding)
         OptionChoice.Add(1);
      else
         OptionChoice.Sub(1);
      Details[1] = Options[OptionChoice.Value];
   }

   public void ChangeValue(int x)
   {
      OptionChoice.Value = x;
      Details[1] = Options[OptionChoice.Value];
   }
}
