namespace SlaamMono.Library.ResourceManagement
{
    public class TextureRequest
    {
        public string TextureName { get; private set; }

        public TextureRequest(string textureName)
        {
            TextureName = textureName;
        }
    }
}
