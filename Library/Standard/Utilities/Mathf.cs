
//Mathf


using System;

namespace Cashmere.Library.Standard.Utilities
{
  public static class Mathf
  {
    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
      if (val.CompareTo(min) < 0)
        return min;
      return val.CompareTo(max) > 0 ? max : val;
    }
  }
}
