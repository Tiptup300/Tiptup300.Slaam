using Microsoft.Xna.Framework;

namespace Tiptup300.Slaam.Library.Input;

public class InputService : IInputService
{
   private InputDevice[] _players;

   public void Initialize()
   {
      _players = new InputDevice[1];
      for (int i = 0; i < _players.Length; i++)
      {
         _players[i] = new InputDevice(InputDeviceType.Controller, (ExtendedPlayerIndex)i, -1);
      }
   }

   public void Update()
   {
      for (int i = 0; i < _players.Length; i++)
      {
         _players[i].Update();
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
      for (int i = 0; i < _players.Length; i++)
      {
         if (_players[i].PlayerIndex == playerIndex)
         {
            return i;
         }
      }

      return -1;
   }

}
