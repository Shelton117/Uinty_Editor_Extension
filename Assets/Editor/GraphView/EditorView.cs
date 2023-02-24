using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.graph_view
{
    public class EditorView : GraphView
    {
        public EditorView() {
            // �����Graph����Zoom in/out
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // ������קContent
            this.AddManipulator(new ContentDragger());
            // ����Selection�������
            this.AddManipulator(new SelectionDragger());
            // GraphView������п�ѡ
            this.AddManipulator(new RectangleSelector());

            Init();
        }

        void Init()
        {
            // 1. ����StartNode�������ú���position
            var startNode = GenEntryPointNode();
            // 2. ��node���뵽GraphView��
            AddElement(startNode);
            // 3. ��StartNode���Output Port
            var port = GenPortForNode(startNode, Direction.Output);
            // 4. ��output����
            port.portName = "Next";
            // 5. ���뵽StartNode��outputContainer��
            startNode.outputContainer.Add(port);

            startNode.RefreshExpandedState();
            startNode.RefreshPorts();
        }

        // ====================== ��DialogueGraphView���� ==========================
        public void AddDialogueNode(string nodeName)
        {
            // 1. ����Node
            EditorNode node = new EditorNode
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString(),
                Text = nodeName,
                Entry = false
            };
            node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));

            // 2. Ϊ�䴴��InputPort
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

        // Ϊ�ڵ�n����input port����output port
        // Direction: ��һ���򵥵�ö�٣���ΪInput��Output����
        private Port GenPortForNode(Node node, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
        {
            // OrientationҲ�Ǹ��򵥵�ö�٣���ΪHorizontal��Vertical���֣�port������������float
            return node.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
        }
    }

    // ����dialogue graph�ĵײ�ڵ���
    public class EditorNode : Node
    {
        public string GUID;
        public string Text;
        public bool Entry = false;
    }
}