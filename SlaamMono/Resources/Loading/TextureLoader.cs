using Microsoft.Xna.Framework.Graphics;

namespace SlaamMono.Resources.Loading
{
    public class TextureLoader : ITextureLoader
    {

        public Texture2D Load(string filePath)
        {
            Texture2D output;

            output = SlaamGame.Content.Load<Texture2D>(filePath);

            return output;
        }
    }
}
