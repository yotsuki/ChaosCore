using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChaosCore.CommonLib
{
    public class MD5Ex
    {
#if _SERVICE
        public static string GetMd5HashForText(string input)
        {
            return GetMd5Hash(GetMd5Hash(input, string.Empty));
        }

        public static string GetMd5Hash(string input, string salt = "WYMSzaq1@WSXcde3$RFV")
#else
        public static string GetMd5Hash(string input, string salt = "")
#endif
        {
            using (MD5 md5Hash = MD5.Create()) {
                return GetMd5Hash(md5Hash, input + salt);
            }
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        // Verify a hash against a string. 
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash)) {
                return true;
            }
            else {
                return false;
            }
        }

        public static byte[] RawMd5(byte[] raw)
        {
            using (MD5 md5Hash = MD5.Create()) {
                byte[] data = md5Hash.ComputeHash(raw);
                return data;
            }
        }

        public static bool VerifyMd5Hash(byte[] raw , byte[] hash)
        {
            if(raw == null || hash == null) {
                return false;
            }
            var md5 = RawMd5(raw);
            if(md5.Length != hash.Length) {
                return false;
            }
            for(int i = 0; i < md5.Length; i++) {
                if(md5[i]!= hash[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}
