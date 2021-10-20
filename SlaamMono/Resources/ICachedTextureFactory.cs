namespace SlaamMono.Resources
{
    public interface ICachedTextureFactory
    {
        CachedTexture BuildCachedTexture(string filePath);
    }
}