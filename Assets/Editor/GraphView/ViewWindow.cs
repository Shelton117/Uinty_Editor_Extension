using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.graph_view
{
    public class ViewWindow : EditorWindow
    {
        EditorView _view;

        public ViewWindow()
        {
            
            
        }

        private void OnEnable()
        {
            _view = new EditorView { name = "EditorView" };
            _view.StretchToParentSize();
            rootVisualElement.Add(_view);

            // ��������
            StyleSheet s = Resources.Load<StyleSheet>("GraphView");
            rootVisualElement.styleSheets.Add(s);

            ShowMenu();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_view);
        }

        private void OnGUI()
        {
            
        }

        [MenuItem("Tools/Graph_View")]
        static void Open()
        {
            var win = GetWindow<ViewWindow>();
            win.titleContent = new UnityEngine.GUIContent("Graph View");
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