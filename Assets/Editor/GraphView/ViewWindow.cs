using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Shel.graph_view
{
    public class ViewWindow : EditorWindow
    {
        EditorView _view;

        [MenuItem("Tools/Graph_View")]
        static void Open()
        {
            var win = GetWindow<ViewWindow>();
            win.titleContent = new UnityEngine.GUIContent("Graph_View");
        }

        private void OnEnable()
        {
            _view = new EditorView { name = "EditorView" };
            _view.StretchToParentSize();
            rootVisualElement.Add(_view);

            //  ��������漰���˵����ã�����Ӧ�÷ŵ�DialogueGraphWindow����
            // ���Toolbar����UnityEditor.UIElements��
            Toolbar toolbar = new Toolbar();
            //����lambda��������������ť�����ĺ�������
            Button btn = new Button(
                clickEvent: () =>
                {
                    _view.AddDialogueNode("Dialogue");
                });
            btn.text = "Add Dialogue Node";
            toolbar.Add(btn);
            rootVisualElement.Add(toolbar);

        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_view);
        }

        private void OnGUI()
        {
            
        }
    }
}