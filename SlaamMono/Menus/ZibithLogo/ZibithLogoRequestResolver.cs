using SlaamMono.Library.ResourceManagement;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus.ZibithLogo
{
    public class ZibithLogoRequestResolver : IResolver<ZibithLogoRequest, IState>
    {
        private readonly IResolver<TextureRequest, CachedTexture> _textureRequest;

        public ZibithLogoRequestResolver(IResolver<TextureRequest, CachedTexture> textureRequest)
        {
            _textureRequest = textureRequest;
        }

        public IState Resolve(ZibithLogoRequest request)
        {
            ZibithLogoState output;

            output = new ZibithLogoState()
            {
                StateIndex = 0,
                BackgroundTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogoBG")),
                LogoTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogo"))
            };

            return output;
        }
    }
}
