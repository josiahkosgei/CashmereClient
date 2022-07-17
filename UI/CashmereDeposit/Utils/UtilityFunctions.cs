
// Type: CashmereDeposit.Utils.UtilityFunctions




using System;
using System.Linq;
using System.Reflection;

namespace CashmereDeposit.Utils
{
  public static class UtilityFunctions
  {
    public static string SanitiseString(this string input) => new string(input.Where(new Func<char, bool>(char.IsLetterOrDigit)).ToArray());

    public static object GetPropertyByString(object value, string path)
    {
      try
      {
        Type type = value.GetType();
        string str = path;
        char[] chArray = new char[1]{ '.' };
        foreach (string name in str.Split(chArray).Skip(1).ToList())
        {
          PropertyInfo property = type.GetProperty(name);
          value = property.GetValue(value);
          type = property.PropertyType;
        }
        return value;
      }
      catch (Exception ex1)
      {
        try
        {
          Type type = value.GetType();
          string str = path;
          char[] chArray = new char[1]{ '.' };
          foreach (string name in str.Split(chArray).Skip(1).ToList())
          {
            FieldInfo field = type.GetField(name);
            value = field.GetValue(value);
            type = field.FieldType;
          }
          return value;
        }
        catch (Exception ex2)
        {
          return "{invalid_token: " + path + "}";
        }
      }
    }
  }
}
