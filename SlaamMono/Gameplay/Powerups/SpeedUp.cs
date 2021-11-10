using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using SlaamMono.ResourceManagement;
using System;

namespace SlaamMono.Gameplay.Powerups
{
    public class SpeedUp : Powerup
    {
        private int PowerupIndex = 1;
        private CharacterActor ParentCharacter;
        private TimeSpan CurrentTime;

        private const float Multiplyer = 1.5f;
        private readonly TimeSpan TimeLasting = new TimeSpan(0, 0, 10);

        public SpeedUp(CharacterActor parent, IResources resources)
            : base(DialogStrings.SpeedUpName, new CachedTexture[] { resources.GetTexture("SpeedUp"), resources.GetTexture("SpeedUp0") }, PowerupUse.Evasion)
        {
            ParentCharacter = parent;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection)
        {
            Active = true;
            CurrentTime = TimeLasting;
            ParentCharacter.SpeedMultiplyer[PowerupIndex] = Multiplyer;
        }

        public override void UpdateAttack()
        {
            CurrentTime -= FrameRateDirector.MovementFactorTimeSpan;

            ParentCharacter.SpeedMultiplyer[PowerupIndex] = Multiplyer;

            if (CurrentTime <= TimeSpan.Zero)
                EndAttack();
        }

        public override void EndAttack()
        {
            Active = false;
            ParentCharacter.SpeedMultiplyer[PowerupIndex] = 1f;
            Used = true;
        }

        public override void Draw(SpriteBatch batch)
        {

        }
    }
}
