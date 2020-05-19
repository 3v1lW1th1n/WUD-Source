namespace Supremus.Encryption
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class Encryption
    {
        private static byte[] IV_64 = new byte[] { 0x44, 0xeb, 14, 0x87, 0x59, 11, 0x1b, 0x49 };
        private static byte[] KEY_64 = new byte[] { 0x35, 0x7b, 0x84, 0x43, 0x1c, 0x53, 50, 140 };

        public static string Decrypt(string value)
        {
            string str = "";
            if (value.Length > 0)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(value));
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
                str = new StreamReader(stream2).ReadToEnd();
            }
            return str;
        }

        public static string Encrypt(string value)
        {
            string str = "";
            if (value.Length > 0)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(stream2);
                writer.Write(value);
                writer.Flush();
                stream2.FlushFinalBlock();
                stream.Flush();
                str = Convert.ToBase64String(stream.GetBuffer(), 0, Convert.ToInt32(stream.Length));
            }
            return str;
        }
    }
}

