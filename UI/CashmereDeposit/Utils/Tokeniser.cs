
// Type: CashmereDeposit.Utils.Tokeniser

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CashmereDeposit.Models;
using CashmereDeposit.ViewModels;

namespace CashmereDeposit.Utils
{
  internal class Tokeniser
  {
    protected DeviceConfiguration DeviceConfiguration { get; set; }

    public Tokeniser(DeviceConfiguration deviceConfigurtion) => DeviceConfiguration = deviceConfigurtion;

    public string ReplaceString<TObject>(
      TObject inputObject,
      string Template,
      List<string> AllowedTokens)
    {
      foreach ((string fullToken, string token, string format) tuple in GetTokensFromTemplate(Template))
      {
        string token = tuple.token;
        if (AllowedTokens.Contains("{" + token + "}"))
        {
          string newValue;
          try
          {
            newValue = FormatToken(UtilityFunctions.GetPropertyByString(inputObject, token), tuple.format);
          }
          catch (Exception ex)
          {
            ApplicationViewModel.Log.WarningFormat(GetType().Name + ".ReplaceString()", "Invalid Token Object", "Invalid Token", "Could not generate object from token {0} of {1}: {2}>>{3}>>{4}", token, tuple.fullToken, ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
            newValue = "{Invalid Object from Token: " + token + "}";
          }
          Template = Template.Replace(tuple.fullToken, newValue);
        }
      }
      return Template;
    }

    public static List<(string fullToken, string token, string format)> GetTokensFromTemplate(
      string template)
    {
      List<string> list = new Regex("\\{(.*?)\\}").Matches(template).Cast<Match>().Select(m => m.Value).Distinct().OrderBy(s => s).ToList();
      List<(string, string, string)> result = new List<(string, string, string)>(10);
      Action<string> body = token =>
      {
          string[] strArray = token.Replace("{", "").Replace("}", "").Split(new string[1]
          {
              ":"
          }, StringSplitOptions.RemoveEmptyEntries);
          result.Add((token, strArray[0], strArray.Length > 1 ? strArray[1] : null));
      };
      Parallel.ForEach(list, body);
      return result;
    }

    public string FormatToken<T>(T token, string format = null)
    {
      try
      {
        switch (token)
        {
          case string str:
            return str;
          case DateTime dateTime:
            return dateTime.ToString(format ?? DeviceConfiguration?.APPLICATION_DATE_FORMAT);
          case Guid guid:
            return guid.ToString(format ?? DeviceConfiguration?.APPLICATION_GUID_FORMAT).ToUpperInvariant();
          case double num:
            return num.ToString(format ?? DeviceConfiguration?.APPLICATION_DOUBLE_FORMAT);
          case int num:
            return num.ToString(format ?? DeviceConfiguration?.APPLICATION_INTEGER_FORMAT);
          default:
            return token.ToString();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Exception occurred while formatting token {0} with format {1}", token, format));
      }
    }
  }
}
