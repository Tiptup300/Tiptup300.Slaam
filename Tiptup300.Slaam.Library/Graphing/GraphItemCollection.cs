using System.Collections.Generic;

namespace Tiptup300.Slaam.Library.Graphing;

public class GraphItemCollection : List<GraphItem>
{
   public List<string> Columns = new List<string>();

   public void Add(bool s, params GraphItem[] itms)
   {
      for (int x = 0; x < itms.Length; x++)
         Add(itms[x]);
   }
}
