namespace SlaamMono.Library.Input;

 public interface IInputService
 {
     int GetIndex(ExtendedPlayerIndex playerIndex);
     InputDevice[] GetPlayers();
 }