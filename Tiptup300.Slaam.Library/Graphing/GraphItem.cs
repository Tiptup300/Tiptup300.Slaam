using System.Collections.Generic;

namespace Tiptup300.Slaam.Library.Graphing;

public class GraphItem
{
   public List<string> Details = new List<string>();
   public bool Highlight = false;

   public GraphItem()
   {

   }

   public GraphItem(params string[] str)
   {
      for (int x = 0; x < str.Length; x++)
         Details.Add(str[x]);
   }

   public void Add(bool s, params string[] str)
   {
      for (int x = 0; x < str.Length; x++)
         Details.Add(str[x]);
   }

   public GraphItemSetting ToSetting()
   {
      return (GraphItemSetting)this;
   }
}
