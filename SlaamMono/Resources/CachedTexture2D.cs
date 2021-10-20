using Microsoft.Xna.Framework.Graphics;
using System;

namespace SlaamMono.Resources
{
    public class CachedTexture2D : IDisposable
    {
        private string _filePath;
        private Texture2D _texture;

        public CachedTexture2D(string filePath)
        {
            _filePath = filePath ?? throw new Exception("No Location Specified For Texture");
        }

        public Texture2D Texture
        {
            get
            {
                if (_texture == null || _texture.IsDisposed)
                {
                    _texture = ResourceManager.LoadTexture(_filePath);
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