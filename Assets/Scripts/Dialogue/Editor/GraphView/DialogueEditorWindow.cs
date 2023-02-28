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
            // 相关内容涉及到菜单设置，所以应该放到DialogueGraphWindow类下
            // 这个Toolbar类在UnityEditor.UIElements下
            Toolbar toolbar = new Toolbar();
            //创建lambda函数，代表点击按钮后发生的函数调用
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