using System;
using System.Collections.Generic;
using System.Text;

namespace ZBlade
{
	public class BladeSetup
	{
		/// <summary>
		/// Gets or sets the text displayed in the back button of the blade.
		/// </summary>
		public string BackButtonText { get; set; }

		/// <summary>
		/// Gets or sets the text displayed in the play button of the blade.
		/// </summary>
		public string PlayButtonText { get; set; }

		/// <summary>
		/// Gets or sets the text displayed in the middle button of the blade.
		/// </summary>
		public string MiddleButtonText { get; set; }

		public BladeSetup(string back, string play, string middle)
		{
			BackButtonText = back;
			PlayButtonText = play;
			MiddleButtonText = middle;
		}
	}
}