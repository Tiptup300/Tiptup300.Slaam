using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement;
using SlaamMono.x_;
using System;

namespace SlaamMono.Gameplay.Powerups
{
    public class SlaamPowerup : Powerup
    {
        private CharacterActor ParentCharacter;
        private int PlayerIndex;
        private const int size = 4;

        public SlaamPowerup(GameScreen parentscreen, CharacterActor parentcharacter, int playerindex, IResources resources)
            : base("Slaam!",
                  new CachedTexture[] { resources.GetTexture("Slaam"), resources.GetTexture("Slaam0") }, PowerupUse.Attacking)
        {
            ParentGameScreen = parentscreen;
            PlayerIndex = playerindex;
            AttackingType = true;
            ParentCharacter = parentcharacter;
            AttackingRange = 4;
            AttackingInLine = false;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection)
        {
            Active = true;
        }

        public override void UpdateAttack()
        {
        }

        public override void EndAttack()
        {
            Vector2 Charpos = ParentGameScreen.InterpretCoordinates(ParentCharacter.Position, true);

            for (int x = (int)Charpos.X - (size - 1); x < Charpos.X + size; x++)
            {
                for (int y = (int)Charpos.Y - (size - 1); y < Charpos.Y + size; y++)
                {
                    if (x == (int)Charpos.X && y == (int)Charpos.Y)
                    {

                    }
                    else if (x >= 0 && x < GameGlobals.BOARD_WIDTH && y >= 0 && y < GameGlobals.BOARD_HEIGHT)
                    {
                        ParentGameScreen.x_Tiles[x, y].MarkTile(ParentCharacter.MarkingColor, new TimeSpan(0, 0, 0, 0, (x + y) * 100), false, PlayerIndex);
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
}
