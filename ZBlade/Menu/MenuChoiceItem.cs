using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
	public class MenuChoiceItem : MenuTextItem
	{
		#region Fields

		private List<string> choices;
		private int currentIndex = 0;

		#endregion

		#region Events

		public event EventHandler ValueChanged;

		#endregion

		#region Properties

		public string CurrentChoice
		{
			get { return choices[CurrentIndex]; }
			set { CurrentIndex = choices.IndexOf(value); }
		}

		public int CurrentIndex
		{
			get { return currentIndex; }
			set
			{
				if (choices.Count == 0)
					value = 0;
				else
				{
					while (value < 0)
						value += choices.Count;
					if (value >= choices.Count)
						value %= choices.Count;
				}
				if (currentIndex != value)
				{
					currentIndex = value;
					if (ValueChanged != null)
						ValueChanged(this, null);
				}
			}
		}

		#endregion

		#region Constructors

		public MenuChoiceItem(string text, params string[] allChoices)
			: this(text, 0, allChoices)
		{
		}

		public MenuChoiceItem(string text, int defaultIndex, params string[] allChoices)
			: this(text, 0, allChoices as IEnumerable<string>)
		{
		}

		public MenuChoiceItem(string text, IEnumerable<string> allChoices)
			: this(text, 0, allChoices)
		{
		}

		public MenuChoiceItem(string text, int defaultIndex, IEnumerable<string> allChoices)
            : base(text)
		{
			Text = text;
			choices = new List<string>(allChoices);
			currentIndex = defaultIndex;
		}

		#endregion

		#region MenuItem Overrides

		/*public override void Draw(SpriteBatch batch, Vector2 position, bool isSelected)
		{
			Helpers.DrawString(
				batch,
				ZuneBlade.Font12,
				ToString(),
				position,
				ZuneBlade.Font12.MeasureString(ToString()) / 2f,
				(IsEnabled) ? Color.White : Color.Gray);
		}*/

		public override bool DetectInput(ZuneButtons type)
		{
			if (type == ZuneButtons.DPadLeft)
			{
				CurrentIndex--;
				return true;
			}
			else if (type == ZuneButtons.DPadRight)
			{
				CurrentIndex++;
				return true;
			}

			return false;
		}

		#endregion

		#region Object Overrides

		public override string ToString()
		{
			return Text + ": < " + CurrentChoice + " >";
		}

		#endregion
	}
}
