using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DAF.Core.Security
{
    public class DESEncryptionProvider : IPasswordEncryptionProvider
    {
        private byte[] key;
        private byte[] iv;

        public DESEncryptionProvider(string key, string iv)
        {
            this.key = key.ToUTF8Bytes().Copy(8, true);
            this.iv = iv.ToUTF8Bytes().Copy(8, true);
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return new byte[0];
            
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return new byte[0];
            
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.None;
                using (MemoryStream ms = new MemoryStream(data))
                {
                    byte[] decrypt = new byte[data.Length];
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.Read(decrypt, 0, decrypt.Length);
                        cs.Close();
                    }
                    return decrypt;
                }
            }
        }
    }
}
