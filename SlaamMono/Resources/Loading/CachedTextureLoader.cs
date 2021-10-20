using SlaamMono.Resources.Loading;

namespace SlaamMono.Resources
{
    public class CachedTextureLoader : IFileLoader<CachedTexture>
    {
        private readonly ITextureLoader _textureLoader;

        public CachedTextureLoader(ITextureLoader textureLoader)
        {
            _textureLoader = textureLoader;
        }


        public object Load(string textureFilePath)
        {
            CachedTexture output;

            output = new CachedTexture(textureFilePath, _textureLoader);

            return output;
        }
    }
}
