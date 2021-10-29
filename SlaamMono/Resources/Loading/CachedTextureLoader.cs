using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.ResourceManagement.Loading
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
