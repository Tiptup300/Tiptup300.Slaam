using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
	public class MenuVisualSliderItem : MenuSliderItem
	{
		#region Fields

		Transition barAppear = new Transition(new Vector2(255), new Vector2(0), TimeSpan.FromSeconds(.25));
		bool lastSelected;

		#endregion

		#region Properties

		private string Percentage
		{
			get { return ((CurrentValue / (double)MaximumValue) * 100) + "%"; }
		}

		#endregion

		#region Constructors

		public MenuVisualSliderItem(string name, int minimum, int maximum)
			: base(name, minimum, maximum)
		{
		}

		#endregion

		#region IMenuItem Methods

		public override void Update(GameTime elapsed, bool isSelected)
		{
			if (lastSelected == isSelected)
				barAppear.Update(elapsed.ElapsedGameTime);
			else
				barAppear.Reverse(TimeSpan.FromSeconds(.25));

			lastSelected = isSelected;
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
				new Color(drawColor.R, drawColor.G, drawColor.B, (byte)(drawColor.A - barAppear.Position.X)));

            Color c = ZuneBlade.instance.ProgressBarColor;

            batch.Draw(
                ZuneBlade.WhitePixel,
                new Rectangle((int)position.X-100, (int)position.Y-8, (int)(((double)CurrentValue / MaximumValue) * 200), 16),
                new Color(c.R, c.G, c.B, (byte)(barAppear.Position.X)));

			batch.Draw(
				ZuneBlade.ProgressbarOverlay,
                position,
				null,
				(IsEnabled) 
					? new Color((byte)255, (byte)255, (byte)255, (byte)(barAppear.Position.X))
					: new Color((byte)50, (byte)50, (byte)50, (byte)(barAppear.Position.X)),
				0f,
				new Vector2(ZuneBlade.ProgressbarOverlay.Width, ZuneBlade.ProgressbarOverlay.Height)/2f,
				1f,
				SpriteEffects.None,
				0);
			Helpers.DrawString(
				batch,
				ZuneBlade.Font12,
                ToString(),
				position - new Vector2(0, .5f),
				ZuneBlade.Font12.MeasureString(ToString()) / 2f,
				(IsEnabled) 
					? new Color((byte)255, (byte)255, (byte)255, (byte)(barAppear.Position.X))
					: new Color((byte)50, (byte)50, (byte)50, (byte)(barAppear.Position.X)));
		}

		#endregion

		#region Object Overrides

		public override string ToString()
		{
			return Text + ":  [ " + Percentage + " ]";
		}

		#endregion
	}
}