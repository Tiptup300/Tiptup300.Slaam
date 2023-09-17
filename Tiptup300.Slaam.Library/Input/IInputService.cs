namespace Tiptup300.Slaam.Library.Input;

public interface IInputService
{
   int GetIndex(ExtendedPlayerIndex playerIndex);
   InputDevice[] GetPlayers();
}