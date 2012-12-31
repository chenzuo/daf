using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Security;

namespace DAF.Web.Security
{
    public class DefaultAuthEncryptionProvider : IAuthEncryptionProvider
    {
        private string providerName;

        public DefaultAuthEncryptionProvider(string providerName)
        {
            this.providerName = providerName;
        }

        private IEncryptionProvider CreateEncryptionProvider()
        {
            switch (providerName.ToLower())
            {
                case "des":
                    return new DESEncryptionProvider(Key, IV);
                case "aes":
                    return new AESEncryptionProvider(Key, IV);
                default:
                    return new PlainEncryptionProvider();
            }
        }

        public string Key { get; set; }

        public string IV { get; set; }

        public byte[] Encrypt(byte[] data)
        {
            var encryptor = CreateEncryptionProvider();
            return encryptor.Encrypt(data);
        }

        public byte[] Decrypt(byte[] data)
        {
            var encryptor = CreateEncryptionProvider();
            return encryptor.Decrypt(data);
        }
    }
}
