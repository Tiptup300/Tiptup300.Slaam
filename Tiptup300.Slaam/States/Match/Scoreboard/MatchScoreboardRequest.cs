using Microsoft.Xna.Framework;
using System.Tiptup300.Primitives;
using Tiptup300.Slaam.States.Match.Actors;
using Tiptup300.Slaam.States.Match.Misc;

namespace Tiptup300.Slaam.States.Match.Scoreboard;

public class MatchScoreboardRequest : IRequest
{
   public Vector2 Position { get; private set; }
   public CharacterActor Character { get; private set; }
   public GameType GameType { get; private set; }

   public MatchScoreboardRequest(Vector2 position, CharacterActor character, GameType gameType)
   {
      Position = position;
      Character = character;
      GameType = gameType;
   }

}
