using System;
using System.Collections.Generic;
using System.Text;

namespace ZBlade
{
	/// <summary>
	/// The current or transitioning status of the blade.
	/// </summary>
	public enum BladeStatus
	{
		/// <summary>
		/// The blade is either out or transitioning to the out state.
		/// </summary>
		Out,

		/// <summary>
		/// The blade is either in or transitioning to the in state.
		/// </summary>
		In,

		/// <summary>
		/// The blade is either hidden or transitioning to the hidden state.
		/// </summary>
		Hidden,

        KeyOut,
	}
}