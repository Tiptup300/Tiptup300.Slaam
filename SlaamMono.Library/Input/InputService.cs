using Microsoft.Xna.Framework;

namespace SlaamMono.Library.Input
{
    public class InputService : IInputService
    {
        public static IInputService Instance;

        private InputDevice[] _players;

        private GamePadHelper _playerOneGamePad = new GamePadHelper(PlayerIndex.One);
        private KeyboardHelper _keyboard = new KeyboardHelper();

        public void Initialize()
        {
            _players = new InputDevice[1];
            for (int x = 0; x < _players.Length; x++)
            {
                _players[x] = new InputDevice(InputDeviceType.Controller, (ExtendedPlayerIndex)x, -1);
            }

            Instance = this;
        }

        public void Update()
        {
            _playerOneGamePad.Update();
            _keyboard.Update();

            for (int idx = 0; idx < _players.Length; idx++)
            {
                _players[idx].Update();
            }
        }

        public InputDevice[] GetPlayers()
        {
            return _players;
        }

        /// <summary>
        /// Gets Input Index from the inputted ExtendedPlayerIndex
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public int GetIndex(ExtendedPlayerIndex playerIndex)
        {
            for (int x = 0; x < _players.Length; x++)
            {
                if (_players[x].PlayerIndex == playerIndex)
                {
                    return x;
                }
            }

            return -1;
        }

    }
}
