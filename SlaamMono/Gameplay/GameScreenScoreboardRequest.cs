using Microsoft.Xna.Framework;
using SlaamMono.Gameplay.Actors;

namespace SlaamMono.Gameplay
{
    public class GameScreenScoreboardRequest
    {
        public Vector2 Position { get; private set; }
        public CharacterActor Character { get; private set; }
        public GameType GameType { get; private set; }

        public GameScreenScoreboardRequest(Vector2 position, CharacterActor character, GameType gameType)
        {
            Position = position;
            Character = character;
            GameType = gameType;
        }

    }
}
