using System;
using System.Collections.Generic;
using System.Text;

namespace Slaam
{
    class RandomList<T> : List<T>
    {
        private Random rand = new Random();

        public void RandomizeList()
        {
            List<T> temp = new List<T>();
            List<int> intsused = new List<int>();

            while (temp.Count != this.Count)
            {
                int x = rand.Next(0, this.Count);
                bool used = false;

                for(int y = 0; y < intsused.Count; y++)
                {
                    if(x == intsused[y])
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

            this.Clear();

            for(int x = 0; x < temp.Count; x++)
                this.Add(temp[x]);
        }
    }
}
