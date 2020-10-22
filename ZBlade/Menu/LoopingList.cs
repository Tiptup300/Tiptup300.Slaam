using System;
using System.Collections.Generic;

namespace ZBlade
{
	public class LoopingList<T> : List<T>
	{
		public new T this[int index]
		{
			get
			{
				if (Count == 0)
					throw new IndexOutOfRangeException();

				List<T> temp = (List<T>)this;

				if (index >= Count)
					index %= Count;

				while (index < 0)
					index += Count;

				return temp[index];
			}
		}
	}
}
