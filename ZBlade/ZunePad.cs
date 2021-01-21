using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ZBlade
{
	[Flags]
	public enum ZuneButtons
	{
		None = 0,
		PlayPause = 1,
		Back = 2,
		PadCenter = 4,
		PadAnywhere = 8,
		DPadUp = 16,
		DPadDown = 32,
		DPadLeft = 64,
		DPadRight = 128,
	}

	public struct ZunePadState
	{
		internal ZuneButtons ButtonsDown { get; set; }
		
		public bool IsTouched { get; internal set; }
		public bool IsTapped { get; internal set; }
		public Vector2 Flick { get; internal set; }
		public Vector2 TouchPosition { get; internal set; }

		public bool IsButtonDown(ZuneButtons button)
		{
			return ((ButtonsDown & button) == button);
		}

		public bool IsButtonUp(ZuneButtons button)
		{
			return !IsButtonDown(button);
		}
	}

#if !ZUNE
	public class ZunePadKeyboardMapping
	{
		public Keys PlayPause = Keys.Space;
		public Keys PadCenter = Keys.Enter;
		public Keys PadAnywhere = Keys.Tab;
		public Keys Back = Keys.Escape;
		public Keys DPadUp = Keys.Up;
		public Keys DPadDown = Keys.Down;
		public Keys DPadLeft = Keys.Left;
		public Keys DPadRight = Keys.Right;
	}
#endif

	public static class ZunePad
	{
		public static GamePadDeadZone DeadZone { get; set; }
		public static bool PollWithTouch { get; set; }

#if !ZUNE
		public static ZunePadKeyboardMapping KeyboardMapping { get; private set; }
#endif

		static ZunePadState zps;

		static TimeSpan flickStartTime;
		static Vector2 flickStart;

		static ZunePad()
		{
#if !ZUNE
			KeyboardMapping = new ZunePadKeyboardMapping();
#endif
			DeadZone = GamePadDeadZone.None;
		}

		/// <summary>
		/// Gets the state of the Zune input allowing for flicks and taps.
		/// </summary>
		/// <param name="gameTime">The current time snapshot</param>
		/// <returns>The new ZunePadState object.</returns>
		public static ZunePadState GetState(GameTime gameTime)
		{
			if (PollWithTouch)
				return GetStateWithTouch(gameTime);
			else
				return GetStateWithoutTouch();
		}

		private static ZunePadState GetStateWithTouch(GameTime gameTime)
		{
			GamePadState gps = GamePad.GetState(PlayerIndex.One, DeadZone);
			Vector2 flick = Vector2.Zero;
			bool tapped = false;

			Vector2 thumbstick = gps.ThumbSticks.Left;
#if ZUNE
			/*if (ZuneBlade.Instance.Orientation == BladeOrientation.Landscape)
			{
				thumbstick.X = -gps.ThumbSticks.Left.Y;
				thumbstick.Y = gps.ThumbSticks.Left.X;
			}*/
#endif

			if (gps.Buttons.LeftStick == ButtonState.Pressed && !zps.IsTouched)
			{
				flickStart = thumbstick;
				flickStartTime = gameTime.TotalGameTime;
			}
			else if (gps.Buttons.LeftStick == ButtonState.Released && zps.IsTouched)
			{
				flick = zps.TouchPosition - flickStart;
				TimeSpan elapsed = gameTime.TotalGameTime - flickStartTime;

				//scale the flick based on how long it took
				flick /= (float)elapsed.TotalSeconds;

				//adjust the .5 and .3 to fit your sensitivity needs. .5 and .3 seem
				//to be pretty decent, but they might need tweaking for some situations
				tapped = (flick.Length() < .5f && elapsed.TotalSeconds < .3f);

				flickStart = Vector2.Zero;
			}

			ZuneButtons buttonsDown = GetButtonsDown(ref gps);

			zps = new ZunePadState()
			{
				IsTapped = tapped,
				IsTouched = gps.Buttons.LeftStick == ButtonState.Pressed,
				Flick = flick,
				TouchPosition = thumbstick,
				ButtonsDown = buttonsDown
			};

			return zps;
		}

		private static ZunePadState GetStateWithoutTouch()
		{
			GamePadState gps = GamePad.GetState(PlayerIndex.One, DeadZone);

			ZuneButtons buttonsDown = GetButtonsDown(ref gps);

			zps = new ZunePadState()
			{
				IsTapped = false,
				IsTouched = false,
				Flick = Vector2.Zero,
				TouchPosition = Vector2.Zero,
				ButtonsDown = buttonsDown
			};

			return zps;
		}

		private static ZuneButtons GetButtonsDown(ref GamePadState gps)
		{
			ZuneButtons buttonsDown = ZuneButtons.None;
			if (gps.Buttons.A == ButtonState.Pressed)
				buttonsDown |= ZuneButtons.PadCenter;
			if (gps.Buttons.LeftShoulder == ButtonState.Pressed)
				buttonsDown |= ZuneButtons.PadAnywhere;
			if (gps.Buttons.B == ButtonState.Pressed)
				buttonsDown |= ZuneButtons.PlayPause;
			if (gps.Buttons.Back == ButtonState.Pressed)
				buttonsDown |= ZuneButtons.Back;

#if !ZUNE
			KeyboardState ks = Keyboard.GetState();
			if (ks.IsKeyDown(KeyboardMapping.DPadUp))
				buttonsDown |= ZuneButtons.DPadUp;
			if (ks.IsKeyDown(KeyboardMapping.DPadDown))
				buttonsDown |= ZuneButtons.DPadDown;
			if (ks.IsKeyDown(KeyboardMapping.DPadLeft))
				buttonsDown |= ZuneButtons.DPadLeft;
			if (ks.IsKeyDown(KeyboardMapping.DPadRight))
				buttonsDown |= ZuneButtons.DPadRight;
			if (ks.IsKeyDown(KeyboardMapping.PadCenter))
				buttonsDown |= ZuneButtons.PadCenter;
			if (ks.IsKeyDown(KeyboardMapping.PadAnywhere))
				buttonsDown |= ZuneButtons.PadAnywhere;
			if (ks.IsKeyDown(KeyboardMapping.PlayPause))
				buttonsDown |= ZuneButtons.PlayPause;
			if (ks.IsKeyDown(KeyboardMapping.Back))
				buttonsDown |= ZuneButtons.Back;
#endif

#if ZUNE
			//if (ZuneBlade.Instance.Orientation == BladeOrientation.Portrait)
			//{
#endif
				if (gps.DPad.Down == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadDown;

				if (gps.DPad.Up == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadUp;

				if (gps.DPad.Left == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadLeft;

				if (gps.DPad.Right == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadRight;

#if ZUNE
                /*}
			else if (ZuneBlade.Instance.Orientation == BladeOrientation.Landscape)
			{
				if (gps.DPad.Left == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadDown;

				if (gps.DPad.Right == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadUp;

				if (gps.DPad.Up == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadLeft;

				if (gps.DPad.Down == ButtonState.Pressed)
					buttonsDown |= ZuneButtons.DPadRight;
			}*/
#endif

			return buttonsDown;
		}
	}
}