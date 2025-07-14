#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

namespace Shovel.Editor
{
    [InitializeOnLoad]
    internal class InitialScene
    {
        static InitialScene()
        {
            var startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Main.unity");
            EditorSceneManager.playModeStartScene = startScene;
        }
    }
}

#endif
