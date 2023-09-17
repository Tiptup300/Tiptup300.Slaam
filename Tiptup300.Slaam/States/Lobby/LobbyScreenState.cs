using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Tiptup300.StateManagement;
using Tiptup300.Slaam.Library.Graphing;
using Tiptup300.Slaam.Library.Widgets;
using Tiptup300.Slaam.PlayerProfiles;

namespace Tiptup300.Slaam.States.Lobby;

public class LobbyScreenState : IState
{
   public Texture2D CurrentBoardTexture { get; set; }
   public int PlayerAmt { get; set; }
   public string[] Dialogs { get; set; }
   public string BoardLocation { get; set; }
   public bool ViewingSettings { get; set; }
   public IntRange MenuChoice { get; set; }

   public Graph MainMenu { get; set; }
   public List<CharacterShell> SetupCharacters { get; set; }
}
