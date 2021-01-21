using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public class MenuTextItem : MenuItem
	{
		#region Properties

		public string Text { get; set; }

		#endregion

		#region Events

		public event EventHandler Activated;

		#endregion

		#region Constructor

		public MenuTextItem(string text)
		{
			Text = text;
		}

		#endregion

		#region MenuItem Overrides

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

		#endregion

		#region Object Overrides

		public override string ToString()
        {
            return Text;
		}

		#endregion
	}
}
