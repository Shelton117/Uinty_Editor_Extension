using UnityEditor.Experimental.GraphView;

namespace Shel.Dialogue
{
    /// <summary>
    /// 工具内显示的节点
    /// GUI
    /// </summary>
    public class DialogueEditorNode : Node
    {
        /// <summary>
        /// 起点标识符
        /// </summary>
        public bool Entry = false;

        public string GUID;
        public string Text;

        public DialogueNodeType Type;
    }
}
