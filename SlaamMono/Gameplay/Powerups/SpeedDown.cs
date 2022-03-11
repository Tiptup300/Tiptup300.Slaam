using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Gameplay.Actors;
using SlaamMono.Gameplay.Boards;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;
using System;

namespace SlaamMono.Gameplay.Powerups
{
    public class SpeedDown : Powerup
    {
        private int CharacterIndex;
        private readonly IFrameTimeService _frameTimeService;
        private int PowerupIndex = 2;
        private const float Multiplier = .50f;
        private readonly TimeSpan TimeLasting = new TimeSpan(0, 0, 10);
        private TimeSpan CurrentTime;

        public SpeedDown(int charindex, IResources resources,
            IFrameTimeService frameTimeService)
            : base(DialogStrings.SpeedDoownName, new CachedTexture[] { resources.GetTexture("SpeedDown"), resources.GetTexture("SpeedDown0") }, PowerupUse.Strategy)
        {
            CharacterIndex = charindex;
            _frameTimeService = frameTimeService;
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

            CurrentTime -= _frameTimeService.GetLatestFrame().MovementFactorTimeSpan;

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
