using System.IO;
using UnityEngine;

using GamePlay.Utility.Nightrol;

public static class SaveLoadSystem
{
    private static string saveFilePath =
        Application.persistentDataPath + "/gameData.json";

    static SaveLoadSystem()
    {
        // Initialize any necessary data or settings here
        Debug.Log("SaveLoadSystem initialized. Save file path: " + saveFilePath);
    }

    public static GameData InitNewGame()
    {
        Debug.Log("Initializing new game data.");

        GameData freshData = new GameData(); // Default constructor will set default values
        SaveGameData(freshData); // Save New Game Data
        return freshData;
    }

    public static void SaveGameData(GameData data)
    {
        if (data == null)
        {
            Debug.LogError("Cannot save null game data.");
            return;
        }

        var config = GameDataManager.Instance.SecurityConfig;

        data.signature = ""; // init signature before HMAC calculation

        // HMAC claculation
        string jsonForHmac = JsonUtility.ToJson(data);
        data.checksum = HMACHelper.ComputeHMAC(jsonForHmac, config.hmacKey);

        // Final JSON serialization
        string json = JsonUtility.ToJson(data);
        byte[] bytesToUTF8 = UTF8Helper.StringToUTF8Bytes(json);

        // AES encryption
        AESSecurityHelper.EncryptAndSave(bytesToUTF8, saveFilePath, config.aesKey, config.aesIV);
    }

    public static GameData LoadGameData()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found. Initializing new game data.");
            return InitNewGame();
        }

        var config = GameDataManager.Instance.SecurityConfig;

        try
        {
            string json;

            // AES decryption
            json = AESSecurityHelper.DecryptAndLoad(File.ReadAllBytes(saveFilePath), saveFilePath, config.aesKey, config.aesIV);

            // JSON parsing
            GameData data = JsonUtility.FromJson<GameData>(json);

            if (data == null)
            {
                Debug.LogError("Failed to parse game data. Initializing new game data.");
                return InitNewGame();
            }

            // HMAC verification
            string originalSignature = data.signature;
            data.signature = ""; // 서명 초기화

            // HMAC calculation for integrity check
            string reCalculatedSignature = 
                HMACHelper.ComputeHMAC(JsonUtility.ToJson(data), config.hmacKey);
            
            if (originalSignature != reCalculatedSignature)
            {
                Debug.LogError("[SaveLoadSystem,HMAC] Data integrity check failed! Possible tampering detected. Initializing new game data.");

                // Data tampering detected, initialize new game data
                return InitNewGame();
            }

            data.signature = originalSignature; // 서명 복원
            Debug.Log("Game data loaded successfully.");

            return data;
        }
        catch
        {
            return InitNewGame(); // Exception occurred, initialize new game data
        }
    }
}