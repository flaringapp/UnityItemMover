namespace Controllers
{
    public enum TextureDepth
    {
        D16 = 16,
        D24 = 24,
        D32 = 32
    }

    public static class Ext
    {
        public static int Value(this TextureDepth depth)
        {
            return (int) depth;
        }
    }
}