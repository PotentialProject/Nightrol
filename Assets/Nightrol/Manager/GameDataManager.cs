using UnityEngine;

namespace GamePlay.Utility.Nightrol
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField] private SecurityConfig securityConfig;
        public static GameDataManager Instance { get; private set; }
        public bool isReady => Data != null;
        public GameData Data { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Data = SaveLoadSystem.LoadGameData();
        }

        public void Save()
        {
            SaveLoadSystem.SaveGameData(Data);
        }

        public SecurityConfig SecurityConfig => securityConfig;
    }
}