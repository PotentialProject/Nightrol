using System;

[Serializable]
public class PlayerData
{
    public int level = 1;
    public int experience = 0;
    public int health = 100;
    public int mana = 50;
    // This Data is Temp Data. Add more player-related fields as needed
}

[Serializable]
public class SettingsData
{
    public float masterVolume = 0.5f;
    // This Data is Temp Data. Add more settings-related fields as needed
}

[Serializable]
public class GameData
{
    public PlayerData playerData = new PlayerData();
    public SettingsData settingsData = new SettingsData();

    public string checksum = "";
    public string signature = "";
}