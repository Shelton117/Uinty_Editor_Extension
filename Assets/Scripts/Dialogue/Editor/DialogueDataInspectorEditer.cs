using UnityEditor;
using UnityEngine;

namespace Shel.Dialogue
{
    //[CustomPropertyDrawer(typeof(Dialogue_SO))]
    //public class DialogueDataInspectorEditer : PropertyDrawer
    //{

    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        //base.OnGUI(position, property, label);
    //        EditorGUI.BeginProperty(position, label, property);
    //        var id = property.FindPropertyRelative("ID");
    //        var author = property.FindPropertyRelative("Author");
    //        var version = property.FindPropertyRelative("Version");
    //        var desctiption = property.FindPropertyRelative("Desctiption");
    //        EditorGUI.EndProperty();
    //    }

    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return base.GetPropertyHeight(property, label);
    //    }
    //}

    /// <summary>
    /// data文件Inspector面板重绘
    /// </summary>
    [CustomEditor(typeof(DialogueData))]
    public class DialogueDataInspectorEditer : UnityEditor.Editor
    {
        private bool isShowDetail;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Edit Data", GUILayout.Height(40)))
                {
                    //NodeEditorWindow.Open(serializedObject.targetObject as XNode.NodeGraph);

                    //DialogueEditorWindow.Open(serializedObject.targetObject as Dialogue_SO);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            isShowDetail = GUILayout.Toggle(isShowDetail, "Show Detail");
            if (isShowDetail)
            {
                GUILayout.Label("Raw data", "BoldLabel");
                base.OnInspectorGUI();
            }
            else
            {
                // TODO：选择部分信息展示
                //var id = serializedObject.FindProperty("ID");
                //var author = serializedObject.FindProperty("Author");
                //var version = serializedObject.FindProperty("Version");
                //var desctiption = serializedObject.FindProperty("Desctiption");

                //EditorGUI.BeginDisabledGroup(true);
                //{
                //    EditorGUILayout.PropertyField(id);
                //    EditorGUILayout.PropertyField(author);
                //    EditorGUILayout.PropertyField(version);
                //    EditorGUILayout.PropertyField(desctiption);
                //}
                //EditorGUI.EndDisabledGroup();

                //serializedObject.ApplyModifiedProperties();
            }
        }
    }
}