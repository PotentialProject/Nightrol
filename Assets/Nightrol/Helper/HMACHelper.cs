using System;
using System.Security.Cryptography;
using System.Text;

public static class HMACHelper
{
    public static string ComputeHMAC(string json, string hmacKey)
    {
        using
        (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hmacKey)))
        {
            byte[] hashBytes = 
                hmac.ComputeHash(Encoding.UTF8.GetBytes(json));
            
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }
}