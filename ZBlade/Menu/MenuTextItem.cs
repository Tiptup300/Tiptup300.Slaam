using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ZBlade
{
    public class MenuTextItem : MenuItem
    {

        public string Text { get; set; }

        public event EventHandler Activated;


        public MenuTextItem(string text, EventHandler onActivated = null, bool isEnabled = true)
        {
            Text = text;
            Activated = onActivated;
            IsEnabled = isEnabled;
        }

        

        public override void Draw(SpriteBatch batch, Vector2 position, bool isSelected)
        {
            Color drawColor = Color.White;

            if (!IsEnabled)
                drawColor = Color.Gray;

            if (!isSelected)
                drawColor = new Color((byte)drawColor.R, (byte)drawColor.G, (byte)drawColor.B, (byte)127);

            Helpers.DrawString(
                batch,
                ZuneBlade.Font12,
                ToString(),
                position,
                ZuneBlade.Font12.MeasureString(ToString()) / 2f,
                drawColor);
        }

        public override bool DetectInput(ZuneButtons type)
        {
            if (type == ZuneButtons.PadCenter && Activated != null)
            {
                Activated(this, null);
                return true;
            }
            return false;
        }

        

        public override string ToString() => Text;
    }
}
