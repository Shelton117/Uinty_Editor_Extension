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
            // 允许对Graph进行Zoom in/out
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // 允许拖拽Content
            this.AddManipulator(new ContentDragger());
            // 允许Selection里的内容
            this.AddManipulator(new SelectionDragger());
            // GraphView允许进行框选
            this.AddManipulator(new RectangleSelector());
        }

        void CreateStartNode()
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

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            // 继承的GraphView里有个Property：ports, 代表graph里所有的port
            ports.ForEach((port) =>
            {
                // 对每一个在graph里的port，进行判断，这里有两个规则：
                // 1. port不可以与自身相连
                // 2. 同一个节点的port之间不可以相连
                if (port != startPort && port.node != startPort.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            // 在我理解，这个函数就是把所有除了startNode里的port都收集起来，放到了List里
            // 所以这个函数能让StartNode的Output port与任何其他的Node的Input port相连（output port应该默认不能与output port相连吧）
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

        // 为节点n创建input port或者output port
        // Direction: 是一个简单的枚举，分为Input和Output两种
        private Port GenPortForNode(Node node, Direction portDir, Port.Capacity capacity = Port.Capacity.Single)
        {
            // Orientation也是个简单的枚举，分为Horizontal和Vertical两种，port的数据类型是float
            return node.InstantiatePort(Orientation.Horizontal, portDir, capacity, typeof(float));
        }

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

            // 2. 为其创建InputPort
            var iport = GenPortForNode(node, Direction.Input, Port.Capacity.Multi);
            iport.portName = "input";
            node.inputContainer.Add(iport);
            node.RefreshExpandedState();
            node.RefreshPorts();

            // StartNode的output接口是这么写的：
            // 一个水平连线的输出接口，类型好像是float
            var startP = node.InstantiatePort(
                Orientation.Horizontal,
                Direction.Output,
                Port.Capacity.Single,
                typeof(float)
            );

            // 而新添加的Node的input接口是这么写的：
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

            // 根据node的outport的数目给新的outport命名
            var count = node.outputContainer.Query("connector").ToList().Count;
            string name = $"Output {count}";
            outPort.portName = name;
            node.outputContainer.Add(outPort);
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }
}