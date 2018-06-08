using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BankBE.Helper
{
    public class TripleDESCustom
    {
        private TripleDESCustom _tripleDES;
        private const int BlockSize = 8;
        private const int CrcByteSize = 8;
        private byte[] tripleDESKey;
        private byte[] iVBytes;

        public TripleDESCustom GetTripleDes()
        {
            return _tripleDES;
        }

        public static byte[] sha256Hashing(string data)
        {
            var sha = new SHA256CryptoServiceProvider();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public void ResolveKey(byte[] key)
        {
            if (key != null && key.Length == 32)
            {
                tripleDESKey = new byte[24];
                iVBytes = new byte[8];
                for (int i = 0; i < 32; i++)
                {
                    if (i < 24)
                    {
                        tripleDESKey[i] = key[i];
                    }
                    else
                    {
                        iVBytes[i - 24] = key[i];
                    }
                }
            }
        }

        public string Encrypt(string key, string data, CipherMode cipherMode)
        {
            var hashData = sha256Hashing(data);
            var hashKey = sha256Hashing(key);
            ResolveKey(hashKey);

            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = tripleDESKey,
                IV = iVBytes,
                Mode = cipherMode
            };

            var enc = tripleDES.CreateEncryptor();
            var resultbuff = enc.TransformFinalBlock(hashData,0,hashData.Length);
            return Convert.ToBase64String(resultbuff);
        }

    }
}