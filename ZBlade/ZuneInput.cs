using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ZBlade
{
    class ZunePadInput
    {
        public ZunePadState LastState;
		public ZunePadState CurrentState;

        public void Update(GameTime gameTime)
        {
            LastState = CurrentState;
            CurrentState = ZunePad.GetState(gameTime);
        }

		public bool IsPressed(ZuneButtons state)
        {
			return CurrentState.IsButtonDown(state) && !LastState.IsButtonDown(state);
        }

        public bool IsPressing(ZuneButtons state)
        {
            return CurrentState.IsButtonDown(state) && LastState.IsButtonDown(state);
        }
    }
}
