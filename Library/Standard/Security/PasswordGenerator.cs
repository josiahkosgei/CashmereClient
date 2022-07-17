
// Type: Cashmere.Library.Standard.Security.PasswordGenerator


using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cashmere.Library.Standard.Security
{
  public static class PasswordGenerator
  {
    public static string Generate(
      uint Minimum_Length = 8,
      uint Lower_Case_length = 1,
      uint Upper_Case_length = 1,
      uint Numeric_length = 1,
      uint Special_length = 1)
    {
      StringBuilder stringBuilder = new StringBuilder();
      Random random = new Random();
      stringBuilder.Append(CreatePassword(Special_length, "!@#$%^&*"));
      stringBuilder.Append(CreatePassword(Numeric_length, "1234567890"));
      stringBuilder.Append(CreatePassword(Upper_Case_length, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"));
      stringBuilder.Append(CreatePassword(Math.Max(Minimum_Length - stringBuilder.Length, Lower_Case_length), "abcdefghijklmnopqrstuvwxyz"));
      return stringBuilder.ToString().Shuffle();
    }

    public static string GenerateToken(long length, string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890") => new(GenerateRandom(length).Select(x => AllowedCharacters[x % AllowedCharacters.Length]).ToArray());

    public static string CreatePassword(long length, string valid)
    {
      StringBuilder stringBuilder = new StringBuilder();
      Random random = new Random();
      while (0L < length--)
        stringBuilder.Append(valid[random.Next(valid.Length)]);
      return stringBuilder.ToString();
    }

    public static byte[] GenerateRandom(long length)
    {
        using RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();
        byte[] data = new byte[length];
        cryptoServiceProvider.GetBytes(data);
        return data;
    }
  }
}
