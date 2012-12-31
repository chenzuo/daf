using System;

namespace DAF.Core.Security
{
    public class PlainEncryptionProvider : IPasswordEncryptionProvider
    {
        public PlainEncryptionProvider()
        {
        }

        public byte[] Encrypt(byte[] data)
        {
            return data;
        }

        public byte[] Decrypt(byte[] data)
        {
            return data;
        }
    }
}
