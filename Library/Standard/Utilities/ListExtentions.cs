
//ListExtentions


using System;
using System.Collections.Generic;

namespace Cashmere.Library.Standard.Utilities
{
  public static class ListExtentions
  {
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
      int count = list.Count;
      while (count > 1)
      {
        --count;
        int index = rng.Next(count + 1);
        T obj = list[index];
        list[index] = list[count];
        list[count] = obj;
      }
    }
  }
}
