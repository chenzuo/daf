using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace DAF.Core
{
    public class SecurityHelper
    {
        private static byte[] AESKey = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
        private static byte[] DESKey = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static string Key = "Coache123456789";
        public static byte[] GetBytes(string key, int byteLength)
        {
            return GetBytes(Encoding.UTF8, key, byteLength);
        }

        public static byte[] GetBytes(Encoding encoding, string key, int byteLength)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            while (key.Length < byteLength)
                key += key;
            return encoding.GetBytes(key.Substring(0, byteLength));
        }

        public static string GetRNGBase64String(int length)
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator generator = RNGCryptoServiceProvider.Create();
            generator.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string GetRNGKey(int length)
        {
            byte[] buff = new byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            return sb.ToString();
        }

        public static string Hash(string encryptString)
        {
            return Hash(encryptString, "SHA1");
        }

        public static string Hash(string encryptString, string hashName)
        {
            byte[] pass = Encoding.Unicode.GetBytes(encryptString);
            HashAlgorithm alg = HashAlgorithm.Create(hashName);
            return Convert.ToBase64String(alg.ComputeHash(pass));
        }

        public static string HashWithSalt(string encryptString, string salt)
        {
            return HashWithSalt(encryptString, salt, "SHA1");
        }

        public static string HashWithSalt(string encryptString, string salt, string hashName)
        {
            byte[] pass = Encoding.Unicode.GetBytes(encryptString);
            byte[] saltb = Convert.FromBase64String(salt);
            byte[] dst = new byte[pass.Length + saltb.Length];
            Buffer.BlockCopy(pass, 0, dst, 0, pass.Length);
            Buffer.BlockCopy(saltb, 0, dst, pass.Length, saltb.Length);
            HashAlgorithm alg = HashAlgorithm.Create(hashName);
            return Convert.ToBase64String(alg.ComputeHash(dst));
        }
        /// <summary>
        ///AES加密 
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="encryptKey">加密关键Key</param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            if (encryptKey == "")
            {
                encryptKey = Key;
            }
            return AESEncrypt(encryptString, encryptKey, AESKey);
        }

        public static string AESEncrypt(string encryptString, string encryptKey, byte[] iv)
        {
            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = iv;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="encryptKey">加密关键Key</param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptString, string decryptKey)
        {
            if (decryptKey == "")
            {
                decryptKey = Key;
            }
            return AESDecrypt(decryptString, decryptKey, AESKey);
        }

        public static string AESDecrypt(string decryptString, string decryptKey, byte[] iv)
        {
            decryptString = decryptString.Replace(" ", "+");
            try
            {
                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 32));
                rijndaelProvider.IV = iv;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">加密字符串</param>
        /// <param name="encryptKey">加密关键Key</param>
        /// <returns></returns>
        public static string DESEncrypt(string encryptString, string encryptKey)
        {
            if (encryptKey == "")
            {
                encryptKey = Key;
            }
            return DESEncrypt(encryptString, encryptKey, DESKey);
        }

        public static string DESEncrypt(string encryptString, string encryptKey, byte[] iv)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = iv;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="decryptKey">解密关键KEY</param>
        /// <returns></returns>
        public static string DESDecrypt(string decryptString, string decryptKey)
        {
            if (decryptKey == "")
            {
                decryptKey = Key;
            }
            return DESDecrypt(decryptString, decryptKey, DESKey);
        }

        public static string DESDecrypt(string decryptString, string decryptKey, byte[] iv)
        {
            decryptString = decryptString.Replace(" ", "+");
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
                byte[] rgbIV = iv;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        public static string GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            while (sb.Length < length)
            {
                sb.Append(Guid.NewGuid().ToString().Replace("-", ""));
            }
            return sb.ToString().Substring(0, length);
        }

        public static string GetMathRandomNumberString(int length, bool trimStartZero)
        {
            StringBuilder sb = new StringBuilder();
            if (trimStartZero)
            {
                while (sb.Length < length)
                {
                    sb.Append(Convert.ToString(Math.Abs(GetNextRandom())).Substring(2).TrimStart('0'));
                }
            }
            else
            {
                while (sb.Length < length)
                {
                    sb.Append(Convert.ToString(Math.Abs(GetNextRandom())).Substring(2));
                }
            }
            return sb.ToString().Substring(0, length);
        }

        public static double GetNextRandom()
        {
            int iset = 0;
            Random r1 = new Random(unchecked((int)DateTime.Now.Ticks));
            Random r2 = new Random(~unchecked((int)DateTime.Now.Ticks));
            double gset = 0.0;
            double fac, rsq, v1, v2;
            if (iset == 0)
            {
                do
                {
                    v1 = 2.0 * r1.NextDouble() - 1.0;
                    v2 = 2.0 * r2.NextDouble() - 1.0;
                    rsq = v1 * v1 + v2 * v2;
                }
                while (rsq >= 1.0 || rsq == 0.0);

                fac = Math.Sqrt(-2.0 * Math.Log(rsq) / rsq);
                gset = v1 * fac;
                iset = 1;
                return v2 * fac;
            }
            else
            {
                iset = 0;
                return gset;
            }
        }

        public static void GenerateRSAKeys(int length, out string publicKey, out string publicAndPrivateKeys)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(length))
            {
                publicKey = rsa.ToXmlString(false);
                publicAndPrivateKeys = rsa.ToXmlString(true);
            }
        }

        public static string RSAGenerateSignature(string encryptString, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(encryptString);
                SHA1Managed sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);

                byte[] b = f.CreateSignature(result);

                return Convert.ToBase64String(b);
            }
        }

        public static bool RSAVerifySignature(string decryptString, string generatedCode, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] key = Convert.FromBase64String(generatedCode);

                SHA1Managed sha = new SHA1Managed();
                byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(decryptString));
                return f.VerifySignature(name, key);
            }
        }
    }

}
