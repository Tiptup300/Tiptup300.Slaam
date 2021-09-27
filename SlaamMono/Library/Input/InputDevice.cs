using Microsoft.Xna.Framework;

namespace SlaamMono.Library.Input
{
    public class InputDevice
    {
        #region Variables

        public ExtendedPlayerIndex PlayerIndex;
        private InputDeviceType Type;
        private GamePadHelper GamePad;
#if !ZUNE
        private KeyboardHelper KeyBoard;
#endif
        private int KeyboardIndex;

        public bool PressedUp;
        public bool PressedDown;
        public bool PressedLeft;
        public bool PressedRight;

        public bool PressingLeft;
        public bool PressingRight;
        public bool PressingUp;
        public bool PressingDown;

        public bool PressedAction;
        public bool PressedAction2;
        public bool PressedBack;
        public bool PressedStart;

        #endregion

        #region Constructor

        public InputDevice(InputDeviceType type, ExtendedPlayerIndex playerIndex, int keyboardIndex)
        {
            if (type == InputDeviceType.Controller)
            {
                GamePad = new GamePadHelper((PlayerIndex)playerIndex);
                Type = type;
            }
            else
            {
                Type = type;
#if !ZUNE
                KeyBoard = new KeyboardHelper();
#endif
                PlayerIndex = playerIndex;
                KeyboardIndex = keyboardIndex;
            }
        }

        #endregion

        #region Update

        public void Update()
        {
#if !ZUNE
            if (Type == InputDeviceType.Keyboard)
            {
                KeyBoard.Update();

                if (KeyboardIndex == 0)
                {
                    PressedUp = KeyBoard.PressedKey(Keys.Up);
                    PressedDown = KeyBoard.PressedKey(Keys.Down);
                    PressedLeft = KeyBoard.PressedKey(Keys.Left);
                    PressedRight = KeyBoard.PressedKey(Keys.Right);

                    PressingLeft = KeyBoard.PressingKey(Keys.Left);
                    PressingRight = KeyBoard.PressingKey(Keys.Right);
                    PressingUp = KeyBoard.PressingKey(Keys.Up);
                    PressingDown = KeyBoard.PressingKey(Keys.Down);

                    PressedAction2 = KeyBoard.PressedKey(Keys.RightShift);
                    PressedAction = KeyBoard.PressedKey(Keys.RightControl);
                    PressedBack = KeyBoard.PressedKey(Keys.Back);
                    PressedStart = KeyBoard.PressedKey(Keys.Enter);
                }
                else if (KeyboardIndex == 1)
                {
                    PressedUp = KeyBoard.PressedKey(Keys.W);
                    PressedDown = KeyBoard.PressedKey(Keys.S);
                    PressedLeft = KeyBoard.PressedKey(Keys.A);
                    PressedRight = KeyBoard.PressedKey(Keys.D);

                    PressingLeft = KeyBoard.PressingKey(Keys.A);
                    PressingRight = KeyBoard.PressingKey(Keys.D);
                    PressingUp = KeyBoard.PressingKey(Keys.W);
                    PressingDown = KeyBoard.PressingKey(Keys.S);

                    PressedAction2 = KeyBoard.PressedKey(Keys.LeftShift);
                    PressedAction = KeyBoard.PressedKey(Keys.LeftControl);
                    PressedBack = KeyBoard.PressedKey(Keys.Tab);
                    PressedStart = KeyBoard.PressedKey(Keys.Space);
                }
            }
            else if(Type == InputDeviceType.Controller)
            {
                GamePad.Update();

                PressedUp = (GamePad.PressedPadUp || GamePad.PressedLeftStickUp);
                PressedDown = (GamePad.PressedPadDown || GamePad.PressedLeftStickDown);
                PressedLeft = (GamePad.PressedPadLeft || GamePad.PressedLeftStickLeft);
                PressedRight = (GamePad.PressedPadRight || GamePad.PressedLeftStickRight);

                PressingLeft = (GamePad.PressingPadLeft || GamePad.PressingLeftStickLeft);
                PressingRight = (GamePad.PressingPadRight || GamePad.PressingLeftStickRight);
                PressingUp = (GamePad.PressingPadUp || GamePad.PressingLeftStickUp);
                PressingDown = (GamePad.PressingPadDown || GamePad.PressingLeftStickDown);

                PressedAction2 = GamePad.PressedB;
                PressedAction = GamePad.PressedA;
                PressedBack = GamePad.PressedBack;
                PressedStart = GamePad.PressedStart;
            }
        
                PressedUp = (GamePad.PressedPadUp || GamePad.PressedLeftStickUp);
                PressedDown = (GamePad.PressedPadDown || GamePad.PressedLeftStickDown);
                PressedLeft = (GamePad.PressedPadLeft || GamePad.PressedLeftStickLeft);
                PressedRight = (GamePad.PressedPadRight || GamePad.PressedLeftStickRight);

                PressingLeft = (GamePad.PressingPadLeft || GamePad.PressingLeftStickLeft);
                PressingRight = (GamePad.PressingPadRight || GamePad.PressingLeftStickRight);
                PressingUp = (GamePad.PressingPadUp || GamePad.PressingLeftStickUp);
                PressingDown = (GamePad.PressingPadDown || GamePad.PressingLeftStickDown);

                PressedAction2 = GamePad.PressedBack;
                PressedAction = GamePad.PressedA;
                PressedBack = false;
                PressedStart = GamePad.PressedB;
            
#else
            GamePad.Update();
            PressedStart = GamePad.PressedB;
            PressedAction2 = GamePad.PressedBack;

            bool HasSquircle = false;//Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(Microsoft.Xna.Framework.PlayerIndex.One).HasLeftXThumbStick;

            if (HasSquircle)
            {
                PressedAction =
                    GamePad.PressedA ||
                    GamePad.PressedPadUp ||
                    GamePad.PressedPadDown ||
                    GamePad.PressedPadLeft ||
                    GamePad.PressedPadRight;

                PressedUp = GamePad.PressedLeftStickUp;
                PressedDown = GamePad.PressedLeftStickDown;
                PressedLeft = GamePad.PressedLeftStickLeft;
                PressedRight = GamePad.PressedLeftStickRight;

                PressingUp = GamePad.PressingLeftStickUp;
                PressingDown = GamePad.PressingLeftStickDown;
                PressingLeft = GamePad.PressingLeftStickLeft;
                PressingRight = GamePad.PressingLeftStickRight;
            }
            else
            {
                PressedAction = GamePad.PressedA;

                PressedUp = GamePad.PressedPadUp;
                PressedDown = GamePad.PressedPadDown;
                PressedLeft = GamePad.PressedPadLeft;
                PressedRight = GamePad.PressedPadRight;

                PressingLeft = GamePad.PressingPadLeft;
                PressingRight = GamePad.PressingPadRight;
                PressingUp = GamePad.PressingPadUp;
                PressingDown = GamePad.PressingPadDown;
            }

#endif
        }

        #endregion
    }
}
