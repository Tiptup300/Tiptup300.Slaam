using SlaamMono.Library.ResourceManagement;
using System;
using ZzziveGameEngine;
using ZzziveGameEngine.StateManagement;

namespace SlaamMono.Menus
{
    public class LogoScreenRequestResolver : IResolver<LogoScreenRequest, IState>
    {
        private readonly IResolver<TextureRequest, CachedTexture> _textureRequest;

        public LogoScreenRequestResolver(IResolver<TextureRequest, CachedTexture> textureRequest)
        {
            _textureRequest = textureRequest;
        }

        public IState Resolve(LogoScreenRequest request)
        {
            LogoScreenState output;

            output = new LogoScreenState()
            {
                StateIndex = 0,
                BackgroundTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogoBG")),
                LogoTexture = _textureRequest.Resolve(new TextureRequest("ZibithLogo"))
            };

            return output;
        }
    }
}
