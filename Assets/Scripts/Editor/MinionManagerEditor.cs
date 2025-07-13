using UnityEditor;
using UnityEngine;

namespace Shovel.Editor
{
    [CustomEditor(typeof(MinionManager))]
    public class MinionManagerEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Handles.color = Color.orangeRed;

            var minionMouse = target as MinionManager;

            if (!minionMouse)
                return;

            Vector3[] spawnPoints = minionMouse.SpawnPoints;

            for (var i = 0; i < spawnPoints.Length; i++)
            {
                Handles.Label(spawnPoints[i], $"{i + 1}", EditorStyles.whiteLargeLabel);

                EditorGUI.BeginChangeCheck();
                Vector3 newPoint = Handles.FreeMoveHandle(spawnPoints[i], 0.3f, Vector3.zero, Handles.CircleHandleCap);

                if (!EditorGUI.EndChangeCheck())
                    continue;

                Undo.RecordObject(this, "Modify minion spawn point");
                spawnPoints[i] = newPoint;

                minionMouse.ResetPositions();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var minionMouse = target as MinionManager;

            if (!minionMouse)
                return;

            if (GUILayout.Button("Spawn 1 Minion"))
                minionMouse.AddMinions(1);
        }
    }
}
