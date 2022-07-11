
//LinqExtensions


using System;
using System.Collections.Generic;
using System.Linq;

namespace CashmereUtil
{
  public static class LinqExtensions
  {
    public static IEnumerable<T> Flatten<T>(
      this IEnumerable<T> source,
      Func<T, IEnumerable<T>> childPropertySelector) => source.Flatten((itemBeingFlattened, objectsBeingFlattened) => childPropertySelector(itemBeingFlattened));

    public static IEnumerable<T> Flatten<T>(
      this IEnumerable<T> source,
      Func<T, IEnumerable<T>, IEnumerable<T>> childPropertySelector) => source.Concat(source.Where(item => childPropertySelector(item, source) != null).SelectMany(itemBeingFlattened => childPropertySelector(itemBeingFlattened, source).Flatten<T>(childPropertySelector)));
  }
}
