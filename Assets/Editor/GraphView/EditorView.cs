using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.graph_view
{
    public class EditorView : GraphView
    {
        Rect currentRect;
        float interval = 20f;

        public EditorView() 
        {
            Init();

            CreateStartNode();
        }

        void Init()
        {
            // �����Graph����Zoom in/out
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // ������קContent
            this.AddManipulator(new ContentDragger());
            // ����Selection�������
            this.AddManipulator(new SelectionDragger());
            // GraphView������п�ѡ
            this.AddManipulator(new RectangleSelector());
        }

        void CreateStartNode()
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

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            // �̳е�GraphView���и�Property��ports, ����graph�����е�port
            ports.ForEach((port) =>
            {
                // ��ÿһ����graph���port�������жϣ���������������
                // 1. port����������������
                // 2. ͬһ���ڵ��port֮�䲻��������
                if (port != startPort && port.node != startPort.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            // ������⣬����������ǰ����г���startNode���port���ռ��������ŵ���List��
            // ���������������StartNode��Output port���κ�������Node��Input port������output portӦ��Ĭ�ϲ�����output port�����ɣ�
            return compatiblePorts;
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

            var rect = new Rect(x: 100, y: 200, width: 100, height: 150);
            currentRect = rect;
            node.SetPosition(rect);

            return node;
        }

        // Ϊ�ڵ�n����input port����output port
        // Direction: ��һ���򵥵�ö�٣���ΪInput��Output����
        private Port GenPortForNode(Node node, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
        {
            // OrientationҲ�Ǹ��򵥵�ö�٣���ΪHorizontal��Vertical���֣�port������������float
            return node.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
        }

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

            // 2. Ϊ�䴴��InputPort
            var iport = GenPortForNode(node, Direction.Input, Port.Capacity.Multi);
            iport.portName = "input";
            node.inputContainer.Add(iport);
            node.RefreshExpandedState();
            node.RefreshPorts();

            // StartNode��output�ӿ�����ôд�ģ�
            // һ��ˮƽ���ߵ�����ӿڣ����ͺ�����float
            var startP = node.InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(float)
            );

            // ������ӵ�Node��input�ӿ�����ôд�ģ�
            var newNodeP = node.InstantiatePort(Orientation.Horizontal,
                Direction.Input,
                Port.Capacity.Multi,
                typeof(float)
            );

            Button btn = new Button(() =>
            {
                AddOutputPort(node);
            });
            btn.text = "Add Output Port";
            node.titleContainer.Add(btn);

            var rect = new Rect(currentRect.x + interval + currentRect.width, currentRect.y, 100, 150);
            currentRect = rect;
            node.SetPosition(rect);

            AddElement(node);
        }

        private void AddOutputPort(EditorNode node)
        {
            var outPort = GenPortForNode(node, Direction.Output);

            // ����node��outport����Ŀ���µ�outport����
            var count = node.outputContainer.Query("connector").ToList().Count;
            string name = $"Output {count}";
            outPort.portName = name;
            node.outputContainer.Add(outPort);
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}