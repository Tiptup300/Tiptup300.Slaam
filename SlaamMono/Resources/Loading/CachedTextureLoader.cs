using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Resources;

namespace SlaamMono.Resources.Loading
{
    public class CachedTextureLoader : IFileLoader<CachedTexture>
    {
        private readonly IFileLoader<Texture2D> _textureLoader;

        public CachedTextureLoader(IFileLoader<Texture2D> textureLoader)
        {
            _textureLoader = textureLoader;
        }

        public object Load(string textureFilePath)
            => new CachedTexture(textureFilePath, _textureLoader);
    }
}
