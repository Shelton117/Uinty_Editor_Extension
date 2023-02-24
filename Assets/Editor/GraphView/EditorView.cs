using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.graph_view
{
    public class EditorView : GraphView
    {
        public EditorView() {
            // 允许对Graph进行Zoom in/out
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // 允许拖拽Content
            this.AddManipulator(new ContentDragger());
            // 允许Selection里的内容
            this.AddManipulator(new SelectionDragger());
            // GraphView允许进行框选
            this.AddManipulator(new RectangleSelector());

            Init();
        }

        void Init()
        {
            // 1. 创建StartNode，并设置好其position
            var startNode = GenEntryPointNode();
            // 2. 把node加入到GraphView里
            AddElement(startNode);
            // 3. 给StartNode添加Output Port
            var port = GenPortForNode(startNode, Direction.Output);
            // 4. 给output改名
            port.portName = "Next";
            // 5. 加入到StartNode的outputContainer里
            startNode.outputContainer.Add(port);

            startNode.RefreshExpandedState();
            startNode.RefreshPorts();
        }

        // ====================== 在DialogueGraphView类内 ==========================
        public void AddDialogueNode(string nodeName)
        {
            // 1. 创建Node
            EditorNode node = new EditorNode
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString(),
                Text = nodeName,
                Entry = false
            };
            node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));

            // 2. 为其创建InputPort
            var iport = GenPortForNode(node, Direction.Input, Port.Capacity.Multi);
            iport.portName = "input";
            node.inputContainer.Add(iport);
            node.RefreshExpandedState();
            node.RefreshPorts();

            AddElement(node);
        }


        private EditorNode GenEntryPointNode()
        {
            EditorNode node = new EditorNode
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                Text = "ENTRYPOINT",
                Entry = true
            };
            node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));

            return node;
        }

        // 为节点n创建input port或者output port
        // Direction: 是一个简单的枚举，分为Input和Output两种
        private Port GenPortForNode(Node node, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
        {
            // Orientation也是个简单的枚举，分为Horizontal和Vertical两种，port的数据类型是float
            return node.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
        }
    }

    // 创建dialogue graph的底层节点类
    public class EditorNode : Node
    {
        public string GUID;
        public string Text;
        public bool Entry = false;
    }
}