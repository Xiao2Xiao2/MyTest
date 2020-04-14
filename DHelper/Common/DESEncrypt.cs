using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DHelper.Common
{
    public class DESEncrypt
    {
        private static string _MyKey 
        { 
            get{
                return "MyTest";
            }
        }
        public static string Encrypt(string Text)
		{
            return DESEncrypt.Encrypt(Text, _MyKey);
		}

		public static string Encrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(Text);
			MD5 mD = MD5.Create();
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(BitConverter.ToString(mD.ComputeHash(Encoding.Default.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(BitConverter.ToString(mD.ComputeHash(Encoding.Default.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = memoryStream.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				stringBuilder.AppendFormat("{0:X2}", b);
			}
			return stringBuilder.ToString();
		}

		public static string Decrypt(string Text)
		{
            return DESEncrypt.Decrypt(Text, _MyKey);
		}

		public static string Decrypt(string Text, string sKey)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			int num = Text.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = Convert.ToInt32(Text.Substring(i * 2, 2), 16);
				array[i] = (byte)num2;
			}
			MD5 mD = MD5.Create();
			dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(BitConverter.ToString(mD.ComputeHash(Encoding.Default.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
			dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(BitConverter.ToString(mD.ComputeHash(Encoding.Default.GetBytes(sKey))).Replace("-", null).Substring(0, 8));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			return Encoding.Default.GetString(memoryStream.ToArray());
		}
    }
}
