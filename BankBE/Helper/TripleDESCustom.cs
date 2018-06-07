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

    }
}