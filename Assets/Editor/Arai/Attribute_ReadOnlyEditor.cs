/*
    Attribute_ReadOnly.cs
    20260802  arai eito
    [ReadOnly]と入力するとInspectorに表示されるが編集不可の状態ができます。
 */
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Attribute_ReadOnly))]
public class Attribute_ReadOnlyEditor : PropertyDrawer
{

    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}