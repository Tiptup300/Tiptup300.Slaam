using System;
using System.Collections.Generic;

namespace SlaamMono.Library
{
    public class RandomList<T> : List<T>
    {
        private Random rand = new Random();

        public void RandomizeList()
        {
            List<T> temp = new List<T>();
            List<int> intsused = new List<int>();

            while (temp.Count != Count)
            {
                int x = rand.Next(0, Count);
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
}
