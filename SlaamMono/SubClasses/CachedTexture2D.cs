using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Resources;
using System;

namespace SlaamMono.SubClasses
{
    public class CachedTexture2D
    {
        public string Location;
        private Texture2D texture;

        public CachedTexture2D()
        {

        }

        public CachedTexture2D(string loc)
        {
            Location = loc;
        }

        public Texture2D Texture
        {
            get
            {
                if (texture == null || texture.IsDisposed)
                {
                    if (Location == null)
                    {
                        throw new Exception("No Location Specified For Texture");
                    }

                    texture = x_Resources.LoadImage(Location);
                }
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public void Dispose()
        {
            if (texture == null)
            {
                return;
            }

            texture.Dispose();
            texture = null;
        }

        public void Load()
        {
            Texture.ToString();
        }

        public int Height { get { return Texture.Height; } }
        public int Width { get { return Texture.Width; } }
    }
}