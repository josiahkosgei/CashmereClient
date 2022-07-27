
//Licensing.LicenseEncryption


using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace CashmereUtil.Licensing
{
    public class LicenseEncryption
    {
        private RSACryptoServiceProvider csp;
        private RSAParameters privKey;
        private string privateKeyString = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Exponent>AQAB</Exponent>\r\n  <Modulus>y2CXzT1FaGD8GlfNc+X/EhmgeuV4otR4ryPdqICjxw/zB1magBigJFoAJPM2odi0Bk2pltLwklCTyz145cdoSItWUcTE4Vfv0BeKaZddcKMrDMeJtbcgVZJ+Lyrorax6MzHHBZBaCT1Kymb5tOhD2Vh5Ksi/j9QvHnGAXI50QN/v+FhMRms8zLg4ItmRhSY/wiUXFlKmPzDbpZfjCoN+gI44Y+JvvZuYd7ae9lbVMkf+l1omSpEnHgZe6unUXRWMAa3Cq7O6e5GU9oK1Tn/RlIRUEv5sLzUpVRhaYzO1wjg8+Jw4Zwt+jkHY6ue1/k4OHuraDYole5cjFQAhi3K/UQ==</Modulus>\r\n  <P>0kNG72T4sySRMYUYwiKEpYpPt6Pl3mzulXydFo+5QJ6obN1KNTJ65gzAMc/xSBcJkWP83W4hNi/Az6u7d1KVUIjMwB0yzIkDdF28baFhzM2CNzsKc81mnBqs94Vhj9uhngR1WaHGvsxOHvSxbc6hG9ZGUWTM0s3Lu1vamK5PgrM=</P>\r\n  <Q>953jzKsW33jUGtV9iM+LnMqabl9cQd39zIhXY62giKXabT0DJwBm1KGBM8nIey/UyoGryeG4oYw7Lb3ByBljCfreWUSxnoNevrwhgPpG3fg1tLzLD3ku3p5LVIVSh7cQkkP/DCPhPf4OT3n5yyVYTeeYQTdN1SC3rWzFMPqBp+s=</Q>\r\n  <DP>zaY1pAGK6ZQTWm7GN2KRgGOV8pQgSPscIyNddWqfEx/atZd+dCdqYsUH2fB7GjpGBmjYMi771sa/+54I3fzsw3b5Y9FrFPfRZCgmGsfkIu7BtIlPgNHd0UKZ+AIB2TVEjovnxwHepFCo+8fyHeciVlquLf+8AVZ9NEPzuq+KXtc=</DP>\r\n  <DQ>sO7MMJnsDSC8hrQYMGMFArMuqEFRnesvCDBAKYSOWjYQns/i6cJ6t+LzfHrp20QxS8ZeZzH4CM84FhYqWn3xa9crfCP+uyJp1+Z8Fjo/2yoZzhy9CEByQjZf43Qkpb8kgy89FoKo1vArb5Tb7dGwiHI9NBR9dBYdyTkYqkfFz6M=</DQ>\r\n  <InverseQ>Wef7JJxAFqqO3L613jgCfe+lpHbCS5reA4YXEFpFmFWFedw2m8jJLmuFpz0fiST3qzm+VUZgQFKYHAPMtQ61hWI1Jmqu/7jaZPNMRL/1aC0xRfltqjfYioJNYQivIvIl0a+oahf0STXrDo+Wuv41/ObLU+oxAlqgQXOLjL3ke/g=</InverseQ>\r\n  <D>Apbzx+LAWABoJWO504B5u37FCtuSKiyfbsd7opIkw7uhwhWzgG8P1vaoatAIAY6TWEMz5h5AfvR0tVFOkUp1Ovs/OaJSSXZHsPQjyI1rXKxPN4Xw7HXs/Pn1Dl+y/8ci9TTsjlcUtmpn4eTmyBVvC7zzKgbNXaT7hp02X7dWpp7NiF0FqZOIOrsJEo88vlG43OOzvyTmN8KJc/CwvHAMBTw10qUJzynnt5F9Df9YA5HgRvf2IHYmFh8At9esm9dd0ndxNWXk4osNaXRVrWBM97QlnTbrpE6oOAe2dw0E9CD2V2XiajispdRtgzB3/SIvx6aS4FBlnl7CcAeIBvQ8ww==</D>\r\n</RSAParameters>\r\n";

        private string GenerateRSAPrivateKeyString()
        {
            StringWriter stringWriter = new StringWriter();
            new XmlSerializer(typeof(RSAParameters)).Serialize(stringWriter, privKey);
            privateKeyString = stringWriter.ToString();
            return privateKeyString;
        }

        private RSAParameters RSAPrivateKeyStringToRSAParameters()
        {
            privKey = (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(new StringReader(privateKeyString));
            return privKey;
        }

        public string Decrypt(string message, bool useOAEP = true)
        {
            byte[] rgb = Convert.FromBase64String(message);
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(RSAPrivateKeyStringToRSAParameters());
            return Encoding.Unicode.GetString(csp.Decrypt(rgb, useOAEP));
        }

        public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (Key == null || Key.Length == 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length == 0)
                throw new ArgumentNullException(nameof(IV));
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Key = Key;
            rijndaelManaged.IV = IV;
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                streamWriter.Write(plainText);
            return memoryStream.ToArray();
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length == 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length == 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length == 0)
                throw new ArgumentNullException(nameof(IV));
            using RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Key = Key;
            rijndaelManaged.IV = IV;
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            using MemoryStream memoryStream = new MemoryStream(cipherText);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
