using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.Dialogue
{
    public class DialogueEditorWindow : EditorWindow
    {
        DialogueEditorView _view;

        public DialogueEditorWindow()
        {
            
        }

        private void OnEnable()
        {
            _view = new DialogueEditorView { name = "EditorView" };
            _view.StretchToParentSize();
            rootVisualElement.Add(_view);

            ShowMenu();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_view);
        }

        private void OnGUI()
        {
            
        }

        [MenuItem("Tools/Dialogue Editor")]
        static void Open()
        {
            var win = GetWindow<DialogueEditorWindow>();
            win.titleContent = new GUIContent("Graph View");
        }

        void ShowMenu()
        {
            // ��������漰���˵����ã�����Ӧ�÷ŵ�DialogueGraphWindow����
            // ���Toolbar����UnityEditor.UIElements��
            Toolbar toolbar = new Toolbar();
            //����lambda��������������ť�����ĺ�������
            Button btn = new Button(
                clickEvent: () =>
                {
                    _view.AddDialogueNode("Dialogue");
                }
            );
            btn.text = "Add Dialogue Node";
            toolbar.Add(btn);
            rootVisualElement.Add(toolbar);
        }
    }
}