using Crabgame.Managers;
using UnityEditor;
using UnityEngine;

namespace Crabgame.Editor
{
    [CustomEditor(typeof(SpawnerManager), editorForChildClasses: true)]
    public class SpawnerEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Handles.color = Color.orangeRed;

            var minionMouse = target as SpawnerManager;

            if (!minionMouse)
                return;

            Vector3[] spawnPoints = minionMouse.SpawnPoints;

            for (var i = 0; i < spawnPoints.Length; i++)
            {
                Handles.Label(spawnPoints[i], $"{i + 1}", EditorStyles.whiteLargeLabel);

                EditorGUI.BeginChangeCheck();

                Vector3 newPoint = Handles.FreeMoveHandle(
                    spawnPoints[i],
                    0.3f,
                    snap: Vector3.one / 32f, // snap doesn't work
                    Handles.CircleHandleCap
                );

                newPoint.x = Mathf.Round(newPoint.x * 32f) / 32f;
                newPoint.y = Mathf.Round(newPoint.y * 32f) / 32f;

                if (!EditorGUI.EndChangeCheck())
                    continue;

                EditorUtility.SetDirty(target);
                Undo.RecordObject(this, "Modify minion spawn point");
                spawnPoints[i] = newPoint;

                minionMouse.ResetPositions();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var spawner = target as SpawnerManager;

            if (!spawner)
                return;

            GUILayout.Space(16);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Spawn 1", EditorStyles.miniButtonLeft))
                spawner.Spawn(1);

            var redText = new GUIStyle(EditorStyles.miniButtonRight);
            redText.normal.textColor = Color.darkOrange;

            if (GUILayout.Button("Clear All", redText))
                spawner.Clear();

            GUILayout.EndHorizontal();
        }
    }
}
