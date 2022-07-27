
//CharExtentions


namespace Cashmere.Library.Standard.Utilities
{
    public static class CharExtentions
    {
        public static bool IsBitSet(this byte b, int pos) => (b & (uint)(1 << pos)) > 0U;
    }
}
