using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    public abstract class MenuItem
	{
		/// <summary>
		/// Returns whether the MenuItem is currently enabled.
		/// </summary>
		public bool IsEnabled { get; set; }

		public MenuItem()
		{
			IsEnabled = true;
		}

        /// <summary>
        /// Updates the MenuItem
        /// </summary>
        /// <param name="timeElapsed"></param>
		public virtual void Update(GameTime timeElapsed, bool isSelected) { }

        /// <summary>
        /// Draws the MenuItem
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="position"></param>
        public abstract void Draw(SpriteBatch batch, Vector2 position, bool isSelected);

        /// <summary>
        /// Tells the MenuItem when input is detected.
        /// </summary>
        /// <param name="inputs">Shows all the currently pressed input.</param>
        /// <returns>Returns whether the menu should blink the highlight or not.</returns>
        public abstract bool DetectInput(ZuneButtons inputs);
	}
}
