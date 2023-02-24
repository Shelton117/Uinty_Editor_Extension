using UnityEditor.Experimental.GraphView;

namespace Shel.graph_view
{
    // 创建dialogue graph的底层节点类
    public class EditorNode : Node
    {
        public string GUID;
        public string Text;
        public bool Entry = false;
    }
}
