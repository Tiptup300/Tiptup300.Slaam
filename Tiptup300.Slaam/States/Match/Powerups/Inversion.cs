using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tiptup300.Slaam.Library.ResourceManagement;
using Tiptup300.Slaam.Library.Timing;
using Tiptup300.Slaam.States.Match.Actors;

namespace Tiptup300.Slaam.States.Match.Powerups;

public class Inversion : Powerup
{
   private int PowerupIndex = 0;
   private int CharacterIndex;
   private readonly IFrameTimeService _frameTimeService;
   private const float Multiplier = -1f;
   private readonly TimeSpan TimeLasting = new TimeSpan(0, 0, 10);
   private TimeSpan CurrentTime;

   public Inversion(int charindex, IResources resources,
       IFrameTimeService frameTimeService)
       : base(
             DialogStrings._["InversionName"],
             new CachedTexture[] {
                   resources.GetTexture("Inversion"),
                   resources.GetTexture("Inversion0") },
             PowerupUse.Strategy)
   {
      CharacterIndex = charindex;
      _frameTimeService = frameTimeService;
   }

   public override void BeginAttack(Vector2 charposition, Direction chardirection, MatchState gameScreenState)
   {
      Active = true;
      CurrentTime = TimeLasting;
      for (int x = 0; x < gameScreenState.Characters.Count; x++)
      {
         if (x != CharacterIndex && gameScreenState.Characters[x] != null)
            gameScreenState.Characters[x].SpeedMultiplyer[PowerupIndex] = Multiplier;
      }
   }

   public override void UpdateAttack(MatchState gameScreenState)
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

   public override void EndAttack(MatchState gameScreenState)
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
