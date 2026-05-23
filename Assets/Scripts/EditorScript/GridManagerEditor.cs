#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;

        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "m_Script", "tileList");

        SerializedProperty tileListProp = serializedObject.FindProperty("tileList");

        if (tileListProp != null && tileListProp.isArray)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tile List (Normalized Chances):", EditorStyles.boldLabel);
            tileListProp.arraySize = EditorGUILayout.IntField("List Size", tileListProp.arraySize);

            if (tileListProp.arraySize > 0)
            {
                float totalChance = 0f;
                for (int i = 0; i < tileListProp.arraySize; i++)
                {
                    var element = tileListProp.GetArrayElementAtIndex(i);
                    var chanceProp = element.FindPropertyRelative("spawnChance");
                    totalChance += chanceProp.floatValue;
                }

                EditorGUI.indentLevel++;
                for (int i = 0; i < tileListProp.arraySize; i++)
                {
                    var element = tileListProp.GetArrayElementAtIndex(i);
                    var prefabProp = element.FindPropertyRelative("tilePrefab");
                    var chanceProp = element.FindPropertyRelative("spawnChance");
                    var limitProp = element.FindPropertyRelative("limitPerMap");

                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.PropertyField(prefabProp);
                    EditorGUILayout.PropertyField(limitProp);
                    EditorGUILayout.PropertyField(chanceProp);

                    if (totalChance > 0)
                    {
                        float normalized = (chanceProp.floatValue / totalChance) * 100f;
                        EditorGUILayout.LabelField("→ Effective Chance: " + normalized.ToString("F1") + "%");
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Total spawn chance is 0 — cannot normalize.", MessageType.Warning);
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif