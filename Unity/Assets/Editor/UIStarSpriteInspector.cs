using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIStarSprite), true)]
public class UIStarSpriteInspector : UISpriteInspector
{

    private bool _foldOutNormalValue = false;
    protected override void DrawCustomProperties()
    {
        base.DrawCustomProperties();
        int count = NGUIEditorTools.DrawProperty("PointsCount", serializedObject, "PointsCount").intValue;
        ListIterator("NormalizedValue", ref _foldOutNormalValue, count);
    }

    private void ListIterator(string propertyPath, ref bool foldOut, int size)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyPath);
        listProperty.arraySize = size;
        foldOut = EditorGUILayout.Foldout(foldOut, listProperty.name);
        if (foldOut)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("" + i, GUILayout.Width(30));
                elementProperty.floatValue = EditorGUILayout.Slider(elementProperty.floatValue, 0, 1);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }
    }
}
