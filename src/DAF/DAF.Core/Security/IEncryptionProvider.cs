using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Security
{
    public interface IEncryptionProvider
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] data);
    }

    public static class IEncryptionProviderExtensions
    {
        public static string Encrypt(this IEncryptionProvider provider, string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;
            return provider.Encrypt(plainText.ToUTF8Bytes()).ToBase64String();
        }

        public static string Decrypt(this IEncryptionProvider provider, string encyptedText)
        {
            if (string.IsNullOrEmpty(encyptedText))
                return string.Empty;
            return provider.Decrypt(encyptedText.ToBase64Bytes()).ToUTF8String();
        }
    }
}
