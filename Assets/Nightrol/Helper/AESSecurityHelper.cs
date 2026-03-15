using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class AESSecurityHelper
{

    public static void EncryptAndSave(byte[] dataToEncrypt, string filePath, string aesKey, string aesIV)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(aesKey);
            byte[] ivBytes = Encoding.UTF8.GetBytes(aesIV);
            
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                Debug.LogError($"[AES] The key length is {keyBytes.Length} bytes. It must be one of 16, 24, or 32 characters!");
                return;
            }

            if (ivBytes.Length != 16)
            {
                Debug.LogError($"[AES] The IV length is {ivBytes.Length} bytes. It must be exactly 16 characters!");
                return;
            }

            aes.Key = Encoding.UTF8.GetBytes(aesKey);
            aes.IV = Encoding.UTF8.GetBytes(aesIV);

            byte[] encrypted = aes.CreateEncryptor()
                .TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);

            File.WriteAllBytes(filePath, encrypted);
            Debug.Log($"[AES] Data saved successfully to: {filePath}");
        }
    }

    public static string DecryptAndLoad(byte[] encryptedData, string filePath, string aesKey, string aesIV)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(aesKey);
            aes.IV = Encoding.UTF8.GetBytes(aesIV);

            byte[] decrypted = aes.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            Debug.Log($"[AES] Data loaded and decrypted successfully from: {filePath}");

            return UTF8Helper.UTF8BytesToString(decrypted);
        }
    }
}