using Microsoft.Xna.Framework;

namespace Tiptup300.Slaam.Library.Input;

public class InputDevice
{
   public ExtendedPlayerIndex PlayerIndex;
   private InputDeviceType Type;
   private GamePadHelper GamePad;
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

         //  KeyBoard = new KeyboardHelper();

         PlayerIndex = playerIndex;
         KeyboardIndex = keyboardIndex;
      }
   }

   public void Update()
   {
      if (Type == InputDeviceType.Keyboard)
      {
      }
      else if (Type == InputDeviceType.Controller)
      {
         GamePad.Update();

         PressedUp = GamePad.PressedPadUp || GamePad.PressedLeftStickUp;
         PressedDown = GamePad.PressedPadDown || GamePad.PressedLeftStickDown;
         PressedLeft = GamePad.PressedPadLeft || GamePad.PressedLeftStickLeft;
         PressedRight = GamePad.PressedPadRight || GamePad.PressedLeftStickRight;

         PressingLeft = GamePad.PressingPadLeft || GamePad.PressingLeftStickLeft;
         PressingRight = GamePad.PressingPadRight || GamePad.PressingLeftStickRight;
         PressingUp = GamePad.PressingPadUp || GamePad.PressingLeftStickUp;
         PressingDown = GamePad.PressingPadDown || GamePad.PressingLeftStickDown;

         PressedAction2 = GamePad.PressedB;
         PressedAction = GamePad.PressedA;
         PressedBack = GamePad.PressedBack;
         PressedStart = GamePad.PressedStart;
      }

      PressedUp = GamePad.PressedPadUp || GamePad.PressedLeftStickUp;
      PressedDown = GamePad.PressedPadDown || GamePad.PressedLeftStickDown;
      PressedLeft = GamePad.PressedPadLeft || GamePad.PressedLeftStickLeft;
      PressedRight = GamePad.PressedPadRight || GamePad.PressedLeftStickRight;

      PressingLeft = GamePad.PressingPadLeft || GamePad.PressingLeftStickLeft;
      PressingRight = GamePad.PressingPadRight || GamePad.PressingLeftStickRight;
      PressingUp = GamePad.PressingPadUp || GamePad.PressingLeftStickUp;
      PressingDown = GamePad.PressingPadDown || GamePad.PressingLeftStickDown;

      PressedAction2 = GamePad.PressedBack;
      PressedAction = GamePad.PressedA;
      PressedBack = false;
      PressedStart = GamePad.PressedB;

   }
}
