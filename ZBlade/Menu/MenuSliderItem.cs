using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
	public class MenuSliderItem : MenuItem
	{
		#region Static Fields

		public static string Fill = ">";
		public static string Space = "=";

		#endregion

		#region Fields

		private int currentValue, minValue, maxValue;

		#endregion

		#region Events

		public event EventHandler ValueChanged;

		#endregion

		#region Properties

        public string Text { get; set; }

		public int MinimumValue 
		{
			get { return minValue; }
			set
			{
				minValue = (int)Math.Min(value, maxValue);
				CurrentValue = CurrentValue;
			}
		}

		public int MaximumValue
		{
			get { return maxValue; }
			set 
			{ 
				maxValue = (int)Math.Max(value, minValue);
				CurrentValue = CurrentValue;
			}
		}

		public int CurrentValue
		{
			get { return currentValue; }
			set 
			{
				value = (int)Math.Max(minValue, Math.Min(value, maxValue));

				if (currentValue != value)
				{
					currentValue = value;
					if (ValueChanged != null)
						ValueChanged(this, null);
				}
			}
		}

		#endregion

		#region Constructors

		public MenuSliderItem(string name, int minimum, int maximum)
		{
			Text = name;
			MinimumValue = minimum;
			MaximumValue = maximum;
		}

		#endregion

		#region MenuItem Overrides

		public override void Draw(SpriteBatch batch, Vector2 position, bool isSelected)
        {
            Helpers.DrawString(
				batch, 
				ZuneBlade.Font12, 
				ToString(),
				position,
				ZuneBlade.Font12.MeasureString(ToString()) / 2f,
				(IsEnabled) ? Color.White : Color.Gray);
        }

		public override bool DetectInput(ZuneButtons type)
        {
			if (type == ZuneButtons.DPadLeft)
				CurrentValue--;
			else if (type == ZuneButtons.DPadRight)
				CurrentValue++;

			if (type == ZuneButtons.PadCenter)
                return true;
            else
                return false;
		}

		#endregion

		#region Object Overrides

		public override string ToString()
        {
            string temp = "";

            for (int x = 0; x < MaximumValue; x++)
            {
                if (x < CurrentValue)
                    temp += Fill+" ";
                else
                    temp += Space+" ";
            }
            return Text + ":  [ " + temp + "]";
		}

		#endregion
	}
}
