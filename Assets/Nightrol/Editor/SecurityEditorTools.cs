using UnityEngine;
using UnityEditor;
using System.IO;

namespace GamePlay.Utility.Nightrol
{
    public class SecurityEditorTools
    {
        [MenuItem("Nightrol/Create GameDataManager")]
        public static void CreateGameDataManager()
        {
            if (Object.FindAnyObjectByType<GameDataManager>() != null)
            {
                EditorUtility.DisplayDialog("Notification", "GameDataManager already exists in the scene!", "OK");
                return;
            }

            GameObject go = new GameObject("GameDataManager");
            go.AddComponent<GameDataManager>();

            Selection.activeGameObject = go;

            Undo.RegisterCreatedObjectUndo(go, "Create GameDataManager");
            Debug.Log("[Nightrol] GameDataManager has been created.");
        }

        [MenuItem("Nightrol/Make Key File")]
        public static void MakeKeyFile()
        {
            string path = "Assets/GameSecurityConfig.asset";
            SecurityConfig config = AssetDatabase.LoadAssetAtPath<SecurityConfig>(path);

            if (config == null)
            {
                config = ScriptableObject.CreateInstance<SecurityConfig>();
                config.GenerateRandomKeys();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                Debug.Log($"[Nightrol] New security settings asset created: {path}");
            } else
            {
                EditorUtility.DisplayDialog("Notification", "SecurityConfig asset already exists!", "OK");
                Debug.Log($"[Nightrol] Security settings asset already exists: {path}");
            }
        }

    }
}