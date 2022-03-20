//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using SlaamMono.Library;
//using SlaamMono.Library.ResourceManagement;

//namespace SlaamMono.x_
//{
//    public class BattleBackground : IBackground
//    {
//        private float _offset = 0f;

//        private readonly CachedTexture _groundTexture;
//        private readonly IFrameTimeService _frameTimeService;

//        public BattleBackground(IResources resourceManager,
//            IFrameTimeService frameTimeService)
//        {
//            _groundTexture = resourceManager.GetTexture("BattleBG");
//            _frameTimeService = frameTimeService;
//        }

//        public void Update()
//        {
//            _offset += (_frameTimeService.GetLatestFrame().MovementFactor * (10f / 100f)) % GameGlobals.DRAWING_GAME_HEIGHT;
//        }

//        public void Draw(SpriteBatch batch)
//        {
//            batch.Draw(_groundTexture.Texture, new Vector2(0, _offset - _groundTexture.Height), Color.White);
//            batch.Draw(_groundTexture.Texture, new Vector2(0, _offset), Color.White);
//        }
//    }
//}
