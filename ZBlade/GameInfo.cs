using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ZBlade
{
	public class GameInfo
	{
		/// <summary>
		/// The name of the current game. Shows in bold 12px
		/// </summary>
		public string GameName { get; set; }

		/// <summary>
		/// The author/studio of the current game. Shows in normal 10px. (Can also put other strings)
		/// </summary>
		public string GameAuthor { get; set; }

		/// <summary>
		/// The icon of the current game or author. Draws at 23px * 23px. Other sizes will work, but will be scaled.
		/// </summary>
		public Texture2D GameIcon { get; set; }

		public GameInfo(string name, string author, Texture2D icon)
		{
			GameName = name;
			GameAuthor = author;
			GameIcon = icon;
		}
	}
}