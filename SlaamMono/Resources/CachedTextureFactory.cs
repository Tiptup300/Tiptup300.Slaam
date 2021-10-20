using SlaamMono.Resources.Loading;

namespace SlaamMono.Resources
{
    public class CachedTextureFactory : ICachedTextureFactory
    {
        private readonly ITextureLoader _textureLoader;

        public CachedTextureFactory(ITextureLoader textureLoader)
        {
            _textureLoader = textureLoader;
        }


        public CachedTexture BuildCachedTexture(string textureFilePath)
        {
            CachedTexture output;

            output = new CachedTexture(textureFilePath, _textureLoader);

            return output;
        }
    }
}
