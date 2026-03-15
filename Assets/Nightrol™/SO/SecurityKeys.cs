using UnityEngine;
using System.Security.Cryptography;
using System;

namespace Utility.Nightrol
{
    [CreateAssetMenu(fileName = "AESConfig", menuName = "Security/AES Config")]
    public class SecurityConfig : ScriptableObject
    {
        public string aesKey = "0000000000000000"; // Default value
        public string aesIV = "0000000000000000"; // Default value
        public string hmacKey = "00000000000000000000000000000000"; // Default value

        [ContextMenu("Generate New Random Keys")]
        public void GenerateRandomKeys()
        {
            aesKey = GenerateRandomString(16); // 128bit
            aesIV = GenerateRandomString(16);  // 16byte 고정
            hmacKey = GenerateRandomString(32);
            Debug.Log("Created new random AES Key, IV, and HMAC Key. DO NOT SHARE THESE KEYS");
        }

        private string GenerateRandomString(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes).Substring(0, length);
        }
    }
}