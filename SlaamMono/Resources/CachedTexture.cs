using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources.Loading;
using System;

namespace SlaamMono.Resources
{
    public class CachedTexture : IDisposable
    {
        private string _filePath;
        private readonly IFileLoader<Texture2D> _textureLoader;
        private Texture2D _texture;

        public CachedTexture(string filePath, IFileLoader<Texture2D> textureLoader)
        {
            _filePath = filePath;
            _textureLoader = textureLoader;
        }

        public Texture2D Texture
        {
            get
            {
                if (_texture == null || _texture.IsDisposed)
                {
                    _texture = (Texture2D)_textureLoader.Load(_filePath);
                }
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }

        public void Dispose()
        {
            if (_texture == null)
            {
                return;
            }

            _texture.Dispose();
            _texture = null;
        }

        public void Load()
        {
            Texture.ToString();
        }

        public int Height { get { return Texture.Height; } }
        public int Width { get { return Texture.Width; } }
    }
}