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

            //  相关内容涉及到菜单设置，所以应该放到DialogueGraphWindow类下
            // 这个Toolbar类在UnityEditor.UIElements下
            Toolbar toolbar = new Toolbar();
            //创建lambda函数，代表点击按钮后发生的函数调用
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