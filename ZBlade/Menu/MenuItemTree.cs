using System;

namespace ZBlade
{
    public class MenuItemTree : MenuTextItem
    {
        public LoopingList<MenuItem> Nodes { get; set; }
        public MenuItemTree Parent { get; set; }
        public int CurrentIndex { get; set; }

        public MenuItemTree(string text = null, MenuItemTree parent = null, Action<MenuItemTree> onInit = null)
            : base(text)
        {
            Nodes = new LoopingList<MenuItem>();
            Parent = parent;
            onInit?.Invoke(this);
        }
        public override bool DetectInput(ZuneButtons type)
        {
            if (type == ZuneButtons.PadCenter)
                ZuneBlade.CurrentMenu = this;

            return false;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
