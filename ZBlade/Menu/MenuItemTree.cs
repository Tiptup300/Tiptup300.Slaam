using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public class MenuItemTree : MenuTextItem
	{
		#region Properties

		public LoopingList<MenuItem> Nodes { get; set; }
		public string Name { get; set; }
		public MenuItemTree Parent { get; set; }
		public int CurrentIndex { get; set; }

		#endregion

		#region Constructors

		public MenuItemTree(string name)
			: this(name, null)
		{
		}

        public MenuItemTree(string name, MenuItemTree parent)
            : base(name)
        {
			Nodes = new LoopingList<MenuItem>();
            Name = name;
            Parent = parent;
		}

		#endregion

		#region MenuItem Overrides


		public override bool DetectInput(ZuneButtons type)
        {
			if (type == ZuneButtons.PadCenter)
                ZuneBlade.CurrentMenu = this;

            return false;
		}

		#endregion

		#region Object Overrides

		public override string ToString()
        {
            return Name;
		}

		#endregion
	}
}
