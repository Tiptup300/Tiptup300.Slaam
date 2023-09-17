using System.Collections.Generic;

namespace Tiptup300.Slaam.Library.Widgets;

public class RedirectionList<T>
{
   public List<T> ParentList;
   public List<int> Indexs = new List<int>();

   public RedirectionList(List<T> parentlist)
   {
      ParentList = parentlist;
   }

   public void Add(int index)
   {
      Indexs.Add(index);
   }

   public T this[int index]
   {
      get { return ParentList[Indexs[index]]; }
      set { ParentList[Indexs[index]] = value; }
   }

   public int Count
   {
      get { return Indexs.Count; }
   }

   public int GetRealIndex(int index)
   {
      return Indexs[index];
   }
}
