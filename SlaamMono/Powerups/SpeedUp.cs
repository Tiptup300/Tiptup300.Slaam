using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Slaam
{
    public class SpeedUp : Powerup
    {
        private int PowerupIndex = 1;
        private Character ParentCharacter;
        private const float Multiplyer = 1.5f;
        private readonly TimeSpan TimeLasting = new TimeSpan(0,0,10);
        private TimeSpan CurrentTime;

        public SpeedUp(Character parent)
            : base(DialogStrings.SpeedUpName,Resources.PU_SpeedUp,PowerupUse.Evasion)
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
            CurrentTime -= FPSManager.MovementFactorTimeSpan;

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
