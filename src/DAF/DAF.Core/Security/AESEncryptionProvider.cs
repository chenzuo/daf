using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DAF.Core.Security
{
    public class AESEncryptionProvider : IPasswordEncryptionProvider
    {
        private RijndaelManaged rijndaelProvider;

        public AESEncryptionProvider(string key, string iv)
        {
            rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = key.ToUTF8Bytes().Copy(32, true);
            rijndaelProvider.IV = iv.ToUTF8Bytes().Copy(16, true);
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return new byte[0];

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijndaelProvider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                return ms.ToArray();
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return new byte[0];

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijndaelProvider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }

                return ms.ToArray();
            }
        }
    }
}
