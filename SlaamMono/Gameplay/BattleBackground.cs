using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.x_
{
    public class BattleBackground : IBackground
    {
        private float _offset = 0f;

        private readonly CachedTexture _groundTexture;

        public BattleBackground(IResources resourceManager)
        {
            _groundTexture = resourceManager.GetTexture("BattleBG");
        }

        public void Update()
        {
            _offset += (FrameRateDirector.MovementFactor * (10f / 100f)) % GameGlobals.DRAWING_GAME_HEIGHT;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_groundTexture.Texture, new Vector2(0, _offset - _groundTexture.Height), Color.White);
            batch.Draw(_groundTexture.Texture, new Vector2(0, _offset), Color.White);
        }
    }
}
