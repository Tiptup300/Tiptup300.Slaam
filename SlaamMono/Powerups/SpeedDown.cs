using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SlaamMono
{
    public class SpeedDown : Powerup
    {
        private int CharacterIndex;
        private int PowerupIndex = 2;
        private const float Multiplier = .50f;
        private readonly TimeSpan TimeLasting = new TimeSpan(0, 0, 10);
        private TimeSpan CurrentTime;

        public SpeedDown(GameScreen parentscreen, int charindex)
            : base(DialogStrings.SpeedDoownName, Resources.PU_SpeedDown,PowerupUse.Strategy)
        {
            ParentGameScreen = parentscreen;
            CharacterIndex = charindex;
        }

        public override void BeginAttack(Vector2 charposition, Direction chardirection)
        {
            Active = true;
            CurrentTime = TimeLasting;
            for (int x = 0; x < ParentGameScreen.Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.Characters[x] != null)
                    ParentGameScreen.Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }
        }

        public override void UpdateAttack()
        {

            for (int x = 0; x < ParentGameScreen.Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.Characters[x] != null)
                    ParentGameScreen.Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
            }

            CurrentTime -= FrameRateDirector.MovementFactorTimeSpan;

            if (CurrentTime <= TimeSpan.Zero)
                EndAttack();
        }

        public override void EndAttack()
        {
            Active = false; 
            for (int x = 0; x < ParentGameScreen.Characters.Count; x++)
            {
                if (x != CharacterIndex && ParentGameScreen.Characters[x] != null)
                    ParentGameScreen.Characters[x].SpeedMultiplyer[PowerupIndex] = 1f;
            }
            Used = true;
        }

        public override void Draw(SpriteBatch batch)
        {

        }
    }
}
