using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlaamMono.Input
{
    public class KeyboardHelper
    {
        public KeyboardState LastState;
        public KeyboardState CurrentState;

        public KeyboardHelper()
        {
            LastState = Keyboard.GetState();
        }

        public void Update()
        {
            LastState = CurrentState;
            CurrentState = Keyboard.GetState();
        }

        public bool PressedKey(Keys key)
        {
            return (CurrentState.IsKeyDown(key) && !LastState.IsKeyDown(key));
        }

        public bool PressingKey(Keys key)
        {
            return (CurrentState.IsKeyDown(key) && LastState.IsKeyDown(key));
        }

    }
}
