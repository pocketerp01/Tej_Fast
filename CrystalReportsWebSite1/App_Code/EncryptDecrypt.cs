using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;


    public class EncryptDecrypt
    {
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
        public static string Encrypt(string originalString)
        {
            string encrypt = string.Empty;
            try
            {
                if (originalString != null || originalString != "")
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cryptoStream);
                    writer.Write(originalString);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    writer.Flush();
                    encrypt = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return encrypt;
        }
        public static string Decrypt(string cryptedString)
        {
            string decrypt = string.Empty;
            try
            {
                if (!String.IsNullOrEmpty(cryptedString))
                {
                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    cryptedString = cryptedString.Replace(" ", "+");
                    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cryptoStream);
                    decrypt = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
            return decrypt;
        }
    }
