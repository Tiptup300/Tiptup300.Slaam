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

        public Inversion(int charindex, IResources resources)
            : base(
                  DialogStrings.InversionName,
                  new CachedTexture[] {
                      resources.GetTexture("Inversion"),
                      resources.GetTexture("Inversion0") },
                  PowerupUse.Strategy)
        {
            CharacterIndex = charindex;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection, GameScreenState gameScreenState)
        {
            Active = true;
            CurrentTime = TimeLasting;
            for (int x = 0; x < gameScreenState.Characters.Count; x++)
            {
                if (x != CharacterIndex && gameScreenState.Characters[x] != null)
                    gameScreenState.Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }
        }

        public override void UpdateAttack(GameScreenState gameScreenState)
        {
            for (int x = 0; x < gameScreenState.Characters.Count; x++)
            {
                if (x != CharacterIndex && gameScreenState.Characters[x] != null)
                    gameScreenState.Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }

            CurrentTime -= FrameRateDirector.MovementFactorTimeSpan;

            if (CurrentTime <= TimeSpan.Zero)
            {
                EndAttack(gameScreenState);
            }
        }

        public override void EndAttack(GameScreenState gameScreenState)
        {
            Active = false;
            for (int x = 0; x < gameScreenState.Characters.Count; x++)
            {
                if (x != CharacterIndex && gameScreenState.Characters[x] != null)
                    gameScreenState.Characters[x].SpeedMultiplyer[PowerupIndex] = 1f;
            }
            Used = true;
        }

        public override void Draw(SpriteBatch batch)
        {

        }
    }
}
