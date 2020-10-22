using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Slaam
{
    /// <summary>
    /// Helper class to handle input from keyboards and gamepads.
    /// </summary>
    class Input : GameComponent
    {

        #region Variables

        public static InputDevice[] Players;

        private static GamePadHelper gamePad = new GamePadHelper(PlayerIndex.One);
#if !ZUNE
        private static KeyboardHelper keyboard = new KeyboardHelper();
#endif
        #endregion

        #region Constructor

        public Input(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            Players = new InputDevice[1];
            for (int x = 0; x < Players.Length; x++)
            {
                Players[x] = new InputDevice(InputDeviceType.Controller, (ExtendedPlayerIndex)x, -1);
            }
            

            base.Initialize();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            gamePad.Update();
#if !ZUNE
            keyboard.Update();
#endif
            for (int idx = 0; idx < Players.Length; idx++)
                Players[idx].Update();

            base.Update(gameTime);
        }

        #endregion

        #region Quick Device Get Methods

        /// <summary>
        /// Gets Input Index from the inputted ExtendedPlayerIndex
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static int GetIndex(ExtendedPlayerIndex idx)
        {
            for (int x = 0; x < Players.Length; x++)
                if (Players[x].PlayerIndex == idx)
                    return x;

            return -1;
        }

        /// <summary>
        /// Get the first inputted gamepad for generic input.
        /// </summary>
        /// <returns></returns>
        public static GamePadHelper GetGamepad()
        {
            return gamePad;
        }

#if !ZUNE
        /// <summary>
        /// Get the keyboard for generic input.
        /// </summary>
        /// <returns></returns>
        public static KeyboardHelper GetKeyboard()
        {
            return keyboard;
        }
#endif

        #endregion

    }

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

        public InputDevice(InputDeviceType type, ExtendedPlayerIndex idx,int keyidx)
        {
            if (type == InputDeviceType.Controller)
            {
                GamePad = new GamePadHelper((PlayerIndex)idx);
                Type = type;
            }
            else
            {
                Type = type;
#if !ZUNE
                KeyBoard = new KeyboardHelper();
#endif
                PlayerIndex = idx;
                KeyboardIndex = keyidx;
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

                    PressedUp = (GamePad.PressedPadUp);
                    PressedDown = (GamePad.PressedPadDown);
                    PressedLeft = (GamePad.PressedPadLeft);
                    PressedRight = (GamePad.PressedPadRight);

                    PressingLeft = (GamePad.PressingPadLeft);
                    PressingRight = (GamePad.PressingPadRight);
                    PressingUp = (GamePad.PressingPadUp);
                    PressingDown = (GamePad.PressingPadDown);
                }
            
#endif
        }

        #endregion
    }

    public class GamePadHelper
    {
        public PlayerIndex GamepadIndex;

        public GamePadState LastState;
        public GamePadState CurrentState;

        public GamePadHelper(PlayerIndex index)
        {
            GamepadIndex = index;
            LastState = GamePad.GetState(GamepadIndex);
        }

        public void Update()
        {
            LastState = CurrentState;
            CurrentState = GamePad.GetState(GamepadIndex);
        }

        public bool PressedBack { get { return Pressed(CurrentState.Buttons.Back, LastState.Buttons.Back); } }
        public bool PressedStart { get { return Pressed(CurrentState.Buttons.Start, LastState.Buttons.Start); } }

        public bool PressedA { 
            get { 
                bool temp = Pressed(CurrentState.Buttons.A, LastState.Buttons.A);
                return temp;
            } 
        }
        public bool PressedB { get { return Pressed(CurrentState.Buttons.B, LastState.Buttons.B); } }
        public bool PressedX { get { return Pressed(CurrentState.Buttons.X, LastState.Buttons.X); } }
        public bool PressedY { get { return Pressed(CurrentState.Buttons.Y, LastState.Buttons.Y); } }

        public bool PressedPadUp { get { return Pressed(CurrentState.DPad.Up, LastState.DPad.Up); } }
        public bool PressedPadDown { get { return Pressed(CurrentState.DPad.Down, LastState.DPad.Down); } }
        public bool PressedPadLeft { get { return Pressed(CurrentState.DPad.Left, LastState.DPad.Left); } }
        public bool PressedPadRight { get { return Pressed(CurrentState.DPad.Right, LastState.DPad.Right); } }

        public bool PressingPadUp { get { return Pressing(CurrentState.DPad.Up, LastState.DPad.Up); } }
        public bool PressingPadDown { get { return Pressing(CurrentState.DPad.Down, LastState.DPad.Down); } }
        public bool PressingPadLeft { get { return Pressing(CurrentState.DPad.Left, LastState.DPad.Left); } }
        public bool PressingPadRight { get { return Pressing(CurrentState.DPad.Right, LastState.DPad.Right); } }


        public bool PressingLeftShoulder { get { return Pressed(CurrentState.Buttons.LeftShoulder, LastState.Buttons.RightShoulder); } }
        public bool PressingRightShoulder { get { return Pressed(CurrentState.DPad.Up, LastState.DPad.Up); } }

        public bool PressedLeftStickUp { get { return (CurrentState.ThumbSticks.Left.Y >= 0.5f && LastState.ThumbSticks.Left.Y <= 0.5f); } }
        public bool PressedLeftStickDown { get { return (CurrentState.ThumbSticks.Left.Y <= -0.5f && LastState.ThumbSticks.Left.Y >= -0.5f); } }
        public bool PressedLeftStickRight { get { return (CurrentState.ThumbSticks.Left.X >= 0.5f && LastState.ThumbSticks.Left.X <= 0.5f); } }
        public bool PressedLeftStickLeft { get { return (CurrentState.ThumbSticks.Left.X <= -0.5f && LastState.ThumbSticks.Left.X >= -0.5f); } }

        public bool PressingLeftStickUp { get { return (CurrentState.ThumbSticks.Left.Y >= 0.5f); } }
        public bool PressingLeftStickDown { get { return (CurrentState.ThumbSticks.Left.Y <= -0.5f); } }
        public bool PressingLeftStickRight { get { return (CurrentState.ThumbSticks.Left.X >= 0.5f); } }
        public bool PressingLeftStickLeft { get { return (CurrentState.ThumbSticks.Left.X <= -0.5f); } }
        

        private bool Pressed(ButtonState last, ButtonState curr)
        {
            return (last == ButtonState.Released) && (curr == ButtonState.Pressed);
        }

        private bool Pressing(ButtonState last, ButtonState curr)
        {
            return (last == ButtonState.Pressed) && (curr == ButtonState.Pressed);
        }
    }

#if !ZUNE
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
#endif

    #region Enums

    public enum ExtendedPlayerIndex
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
    }

    public enum InputDeviceType
    {
        Controller,
        Keyboard,
        Other,
    }

    #endregion
}
