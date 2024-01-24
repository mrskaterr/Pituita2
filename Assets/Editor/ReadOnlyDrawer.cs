using UnityEditor;
using UnityEngine;
using UnityInspector;
using static UnityEditor.Progress;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        //var previousGUIState = GUI.enabled;
        //GUI.enabled = false;
        //EditorGUI.PropertyField(position, property, label);
        //GUI.enabled = previousGUIState;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label);
        EditorGUI.EndDisabledGroup();
    }
}