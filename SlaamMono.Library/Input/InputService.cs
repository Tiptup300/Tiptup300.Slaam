using Microsoft.Xna.Framework;

namespace SlaamMono.Library.Input
{
    public class InputService
    {
        public static InputService Instance = new InputService();

        public InputDevice[] Players;

        private GamePadHelper _playerOneGamePad = new GamePadHelper(PlayerIndex.One);
        private KeyboardHelper _keyboard = new KeyboardHelper();

        public void Initialize()
        {
            Players = new InputDevice[1];
            for (int x = 0; x < Players.Length; x++)
            {
                Players[x] = new InputDevice(InputDeviceType.Controller, (ExtendedPlayerIndex)x, -1);
            }

            Instance = this;
        }

        public void Update()
        {
            _playerOneGamePad.Update();
            _keyboard.Update();

            for (int idx = 0; idx < Players.Length; idx++)
            {
                Players[idx].Update();
            }
        }

        /// <summary>
        /// Gets Input Index from the inputted ExtendedPlayerIndex
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public int GetIndex(ExtendedPlayerIndex playerIndex)
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

    }
}
