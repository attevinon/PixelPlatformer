using UnityEditor;
using UnityEditor.UI;

namespace PixelCrew.UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_transformToMove"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normalY"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_lowY"));
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}