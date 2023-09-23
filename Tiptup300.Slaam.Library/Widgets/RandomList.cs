namespace Tiptup300.Slaam.Library.Widgets;

public class RandomList<T> : List<T>
{
   private readonly Random _random = new Random();

   public void RandomizeList()
   {
      List<T> temp = new List<T>();
      List<int> intsused = new List<int>();

      while (temp.Count != Count)
      {
         int x = _random.Next(0, Count);
         bool used = false;

         for (int y = 0; y < intsused.Count; y++)
         {
            if (x == intsused[y])
            {
               used = true;
               break;
            }
         }

         if (!used)
         {
            intsused.Add(x);
            temp.Add(this[x]);
         }
      }

      Clear();

      for (int x = 0; x < temp.Count; x++)
         Add(temp[x]);
   }
}
