using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement;
using System;

namespace SlaamMono.Gameplay.Powerups
{
    public class Inversion : Powerup
    {
        private int PowerupIndex = 0;
        private int CharacterIndex;
        private const float Multiplier = -1f;
        private readonly TimeSpan TimeLasting = new TimeSpan(0, 0, 10);
        private TimeSpan CurrentTime;

        public Inversion(GameScreen parentscreen, int charindex, IResources resources)
            : base(
                  DialogStrings.InversionName,
                  new CachedTexture[] {
                      resources.GetTexture("Inversion"),
                      resources.GetTexture("Inversion0") },
                  PowerupUse.Strategy)
        {
            ParentGameScreen = parentscreen;
            CharacterIndex = charindex;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection)
        {
            Active = true;
            CurrentTime = TimeLasting;
            for (int x = 0; x < ParentGameScreen.x_Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.x_Characters[x] != null)
                    ParentGameScreen.x_Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }
        }

        public override void UpdateAttack(Tile[,] tiles)
        {
            for (int x = 0; x < ParentGameScreen.x_Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.x_Characters[x] != null)
                    ParentGameScreen.x_Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }

            CurrentTime -= FrameRateDirector.MovementFactorTimeSpan;

            if (CurrentTime <= TimeSpan.Zero)
                EndAttack(tiles);
        }

        public override void EndAttack(Tile[,] tiles)
        {
            Active = false;
            for (int x = 0; x < ParentGameScreen.x_Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.x_Characters[x] != null)
                    ParentGameScreen.x_Characters[x].SpeedMultiplyer[PowerupIndex] = 1f;
            }
            Used = true;
        }

        public override void Draw(SpriteBatch batch)
        {

        }
    }
}
