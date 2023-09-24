using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Composition.x_;

namespace Tiptup300.Slaam.States.Lobby;

public static class LobbyScreenFunctions
{
   public static GameConfiguration _gameConfiguration => ServiceLocator.Instance.GetService<GameConfiguration>();
   private static Texture2D _defaultBoard;



   public static Texture2D LoadQuickBoard()
   {
      if (_defaultBoard == null)
      {
         // . @State/Logic This needs to be rethought out to deal with the new state/logic 

         //BoardSelectionScreenPerformer viewer = null;//= new BoardSelectionScreen(null, null);
         //new BoardSelectionScreenRequestState(null);
         //viewer.InitializeState();
         //while (!viewer.x_HasFoundBoard)
         //{
         //    viewer.Perform();
         //}
         //_defaultBoard = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + GameGlobals.TEXTURE_FILE_PATH + viewer.x_IsValidBoard);
         //viewer.Close();
      }

      return _defaultBoard;
   }

   public static void LoadBoard(string boardLocation, LobbyScreenState lobbyScreenState)
   {
      if (lobbyScreenState.CurrentBoardTexture != null && lobbyScreenState.BoardLocation == boardLocation)
      {
         return;
      }

      try
      {
         lobbyScreenState.CurrentBoardTexture = SlaamGame.Content.Load<Texture2D>("content\\Boards\\" + _gameConfiguration.TEXTURE_FILE_PATH + boardLocation);
         lobbyScreenState.BoardLocation = boardLocation;
      }
      catch (FileNotFoundException)
      {

      }
      lobbyScreenState.Dialogs[0] = DialogStrings.CurrentBoard + lobbyScreenState.BoardLocation.Substring(lobbyScreenState.BoardLocation.IndexOf('_') + 1).Replace(".png", "").Replace("boards\\", "");
      if (lobbyScreenState.BoardLocation.IndexOf('_') >= 0)
      {
         lobbyScreenState.Dialogs[1] = DialogStrings.CreatedBy + lobbyScreenState.BoardLocation.Substring(0, lobbyScreenState.BoardLocation.IndexOf('_')).Replace(".png", "").Replace("boards\\", "");
      }
      else
      {
         lobbyScreenState.Dialogs[1] = "";
      }

      _defaultBoard = lobbyScreenState.CurrentBoardTexture;

   }
}
