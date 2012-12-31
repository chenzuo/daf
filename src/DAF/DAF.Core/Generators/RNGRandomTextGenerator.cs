using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DAF.Core.Generators
{
    public class RNGRandomTextGenerator : IRandomTextGenerator
    {
        public string Generate(string allowedChars, int length)
        {
            Byte[] randomBytes = new Byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[length];
            int allowedCharCount = allowedChars.Length;

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        public string Name
        {
            get { return "RNGRandomTextGenerator"; }
        }
    }
}
