using UnityEditor.Experimental.GraphView;

namespace Shel.Dialogue
{
    /// <summary>
    /// ��������ʾ�Ľڵ�
    /// GUI
    /// </summary>
    public class DialogueEditorNode : Node
    {
        /// <summary>
        /// ����ʶ��
        /// </summary>
        public bool Entry = false;

        public string GUID;
        public string Text;

        public DialogueNodeType Type;
    }
}
