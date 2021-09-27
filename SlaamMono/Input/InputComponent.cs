using Microsoft.Xna.Framework;

namespace SlaamMono.Input
{
    class InputComponent : GameComponent
    {
        public static InputDevice[] Players;

        private static GamePadHelper playerOneGamePad = new GamePadHelper(PlayerIndex.One);
        private static KeyboardHelper keyboard = new KeyboardHelper();

        public InputComponent(Game game)
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

        public override void Update(GameTime gameTime)
        {
            playerOneGamePad.Update();
#if !ZUNE
            keyboard.Update();
#endif
            for (int idx = 0; idx < Players.Length; idx++)
            {
                Players[idx].Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Gets Input Index from the inputted ExtendedPlayerIndex
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public static int GetIndex(ExtendedPlayerIndex playerIndex)
        {
            for (int x = 0; x < Players.Length; x++)
            {
                if (Players[x].PlayerIndex == playerIndex)
                {
                    return x;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get the first inputted gamepad for generic input.
        /// </summary>
        /// <returns></returns>
        public static GamePadHelper GetGamepad()
        {
            return playerOneGamePad;
        }

        /// <summary>
        /// Get the keyboard for generic input.
        /// </summary>
        /// <returns></returns>
        public static KeyboardHelper GetKeyboard()
        {
            return keyboard;
        }

    }




}
