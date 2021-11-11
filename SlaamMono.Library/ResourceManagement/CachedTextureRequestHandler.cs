namespace SlaamMono.Library.ResourceManagement
{
    public class CachedTextureRequestHandler : IResolver<TextureRequest, CachedTexture>
    {
        private readonly State<ResourcesState> _resourcesState;

        public CachedTextureRequestHandler(State<ResourcesState> resourcesState)
        {
            _resourcesState = resourcesState;
        }

        public CachedTexture Execute(TextureRequest request)
        {
            return _resourcesState.Get().Textures[request.TextureName];
        }
    }
}
