using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tiptup300.Slaam.Library.Input;

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

   public bool PressedA
   {
      get
      {
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

   public bool PressedLeftStickUp { get { return CurrentState.ThumbSticks.Left.Y >= 0.5f && LastState.ThumbSticks.Left.Y <= 0.5f; } }
   public bool PressedLeftStickDown { get { return CurrentState.ThumbSticks.Left.Y <= -0.5f && LastState.ThumbSticks.Left.Y >= -0.5f; } }
   public bool PressedLeftStickRight { get { return CurrentState.ThumbSticks.Left.X >= 0.5f && LastState.ThumbSticks.Left.X <= 0.5f; } }
   public bool PressedLeftStickLeft { get { return CurrentState.ThumbSticks.Left.X <= -0.5f && LastState.ThumbSticks.Left.X >= -0.5f; } }

   public bool PressingLeftStickUp { get { return CurrentState.ThumbSticks.Left.Y >= 0.5f; } }
   public bool PressingLeftStickDown { get { return CurrentState.ThumbSticks.Left.Y <= -0.5f; } }
   public bool PressingLeftStickRight { get { return CurrentState.ThumbSticks.Left.X >= 0.5f; } }
   public bool PressingLeftStickLeft { get { return CurrentState.ThumbSticks.Left.X <= -0.5f; } }
   private bool Pressed(ButtonState last, ButtonState curr)
   {
      return last == ButtonState.Released && curr == ButtonState.Pressed;
   }

   private bool Pressing(ButtonState last, ButtonState curr)
   {
      return last == ButtonState.Pressed && curr == ButtonState.Pressed;
   }
}
