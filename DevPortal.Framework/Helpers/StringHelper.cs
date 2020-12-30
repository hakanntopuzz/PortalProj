using System;
using System.Security.Cryptography;
using System.Text;

namespace DevPortal.Framework
{
    public sealed class StringHelper
    {
        public static byte[] EncryptMD5GetByteArray(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] buffer = Encoding.Default.GetBytes(value);
                return md5.ComputeHash(buffer);
            }
        }

        public static byte[] StringToBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string BytesToHexString(byte[] value)
        {
            string s = BitConverter.ToString(value);

            return s.Replace("-", "");
        }

        public static byte[] HexStringToBytes(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}