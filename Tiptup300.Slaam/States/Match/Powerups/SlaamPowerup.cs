using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Composition.x_;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.States.Match.Actors;

namespace Tiptup300.Slaam.States.Match.Powerups;

public class SlaamPowerup : Powerup
{
   private const int SIZE = 4;

   private readonly CharacterActor _parentCharacter;
   private readonly int _playerIndex;
   private GameConfiguration _gameConfiguration => ServiceLocator.Instance.GetService<GameConfiguration>();

   public SlaamPowerup(CharacterActor parentcharacter, int playerindex, IResources resources)
       : base("Slaam!",
             new CachedTexture[] { resources.GetTexture("Slaam"), resources.GetTexture("Slaam0") }, PowerupUse.Attacking)
   {
      _playerIndex = playerindex;
      AttackingType = true;
      _parentCharacter = parentcharacter;
      AttackingRange = 4;
      AttackingInLine = false;
   }

   public override void BeginAttack(Vector2 charposition, Direction chardirection, MatchState gameScreenState)
   {
      Active = true;
   }

   public override void UpdateAttack(MatchState gameScreenState)
   {
   }

   public override void EndAttack(MatchState gameScreenState)
   {
      Vector2 Charpos = MatchFunctions.InterpretCoordinates(gameScreenState, _parentCharacter.Position, true);

      for (int x = (int)Charpos.X - (SIZE - 1); x < Charpos.X + SIZE; x++)
      {
         for (int y = (int)Charpos.Y - (SIZE - 1); y < Charpos.Y + SIZE; y++)
         {
            if (x == (int)Charpos.X && y == (int)Charpos.Y)
            {

            }
            else if (x >= 0 && x < _gameConfiguration.BOARD_WIDTH && y >= 0 && y < _gameConfiguration.BOARD_HEIGHT)
            {
               gameScreenState.Tiles[x, y].MarkTile(_parentCharacter.MarkingColor, new TimeSpan(0, 0, 0, 0, (x + y) * 100), false, _playerIndex);
            }
         }
      }
      Used = true;
      Active = false;
   }

   public override void Draw(SpriteBatch batch)
   {

   }
}
