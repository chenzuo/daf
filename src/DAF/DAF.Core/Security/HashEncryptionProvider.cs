using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace DAF.Core.Security
{
    public class HashEncryptionProvider : IPasswordEncryptionProvider
    {
        private string hashName;

        public HashEncryptionProvider()
            : this("SHA1")
        {
        }

        public HashEncryptionProvider(string hashName)
        {
            this.hashName = hashName;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return new byte[0];

            using (HashAlgorithm alg = HashAlgorithm.Create(hashName))
            {
                return alg.ComputeHash(data);
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
