
//StringCipher


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cashmere.Library.Standard.Utilities
{
  public static class StringCipher
  {
    private const int Keysize = 128;
    private const int DerivationIterations = 1000;

    public static string Encrypt(this string plainText, byte[] passPhrase)
    {
      byte[] bitsOfRandomEntropy1 = GenerateBitsOfRandomEntropy(128);
      byte[] bitsOfRandomEntropy2 = GenerateBitsOfRandomEntropy(128);
      byte[] bytes1 = Encoding.UTF8.GetBytes(plainText);
      using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, bitsOfRandomEntropy1, 1000);
      byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
      using RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.BlockSize = 128;
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      using ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, bitsOfRandomEntropy2);
      using MemoryStream memoryStream = new MemoryStream();
      using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(bytes1, 0, bytes1.Length);
      cryptoStream.FlushFinalBlock();
      byte[] array = bitsOfRandomEntropy1.Concat(bitsOfRandomEntropy2).ToArray().Concat(memoryStream.ToArray()).ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return Convert.ToBase64String(array);
    }

    public static string Decrypt(this string cipherText, byte[] passPhrase)
    {
      byte[] numArray1 = Convert.FromBase64String(cipherText);
      byte[] array1 = numArray1.Take(16).ToArray();
      byte[] array2 = numArray1.Skip(16).Take(16).ToArray();
      byte[] array3 = numArray1.Skip(32).Take(numArray1.Length - 32).ToArray();
      using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, array1, 1000);
      byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
      using RijndaelManaged rijndaelManaged = new RijndaelManaged();
      rijndaelManaged.BlockSize = 128;
      rijndaelManaged.Mode = CipherMode.CBC;
      rijndaelManaged.Padding = PaddingMode.PKCS7;
      using ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2);
      using MemoryStream memoryStream = new MemoryStream(array3);
      using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] numArray2 = new byte[array3.Length];
      int count = cryptoStream.Read(numArray2, 0, numArray2.Length);
      memoryStream.Close();
      cryptoStream.Close();
      return Encoding.UTF8.GetString(numArray2, 0, count);
    }

    private static byte[] GenerateBitsOfRandomEntropy(int bits = 256)
    {
      byte[] data = new byte[bits / 8];
      using RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider();
      cryptoServiceProvider.GetBytes(data);
      return data;
    }
  }
}
