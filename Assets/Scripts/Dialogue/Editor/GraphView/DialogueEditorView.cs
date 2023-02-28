using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shel.Dialogue
{
    public class DialogueEditorView : GraphView
    {
        Rect currentRect;
        float interval = 20f;
        float width, height;

        public DialogueEditorView() 
        {
            // viewTransformChanged += ViewTransformChangedCallBack;
            // graphViewChanged += GraphViewChangedCallBack;

            AddManipulators();
            InitializeView();
            CreateStartNode();

            // Undo.undoRedoPerformed += Reload;
        }

        ~DialogueEditorView()
        {
            // Undo.undoRedoPerformed -= Reload;
        }

        /// <summary>
        /// �Ҽ��˵���
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // �����ѡ�нڵ����ʾ�������Ҽ��˵�
            // base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Text/Dialogue Node", AddDialogueNode);
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

        void AddManipulators()
        {
            // ������קContent
            this.AddManipulator(new ContentDragger());
            // ����Selection�������
            this.AddManipulator(new SelectionDragger());
            // GraphView������п�ѡ
            this.AddManipulator(new RectangleSelector());
            // ���
            this.AddManipulator(new ClickSelector());
        }

        void InitializeView()
        {
            // ����USS�����ļ�
            styleSheets.Add(Resources.Load<StyleSheet>("GraphView"));

            // ��������
            GridBackground gridBackground = new GridBackground();
            gridBackground.name = "GridBackground";
            Insert(0, gridBackground);

            // �����������ŷ�Χ
            SetupZoom(0.5f, 2.0f);

            this.StretchToParentSize();

            // �����������
            // ��ʾ��ǰ���Ŵ�С

            // ����ͼ
            GenerateMiniMap();
            // ������
        }

        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap { anchored = true };
            var cords = contentViewContainer.WorldToLocal(new Vector2(10, 30));
            miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
            Add(miniMap);
        }

        //private void GenerateBlackBoard()
        //{
        //    var blackboard = new Blackboard(_graphView);
        //    blackboard.Add(new BlackboardSection { title = "Exposed Variables" });
        //    blackboard.addItemRequested = _blackboard =>
        //    {
        //        _graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
        //    };
        //    blackboard.editTextRequested = (_blackboard, element, newValue) =>
        //    {
        //        var oldPropertyName = ((BlackboardField)element).text;
        //        if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
        //        {
        //            EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
        //                "OK");
        //            return;
        //        }

        //        var targetIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
        //        _graphView.ExposedProperties[targetIndex].PropertyName = newValue;
        //        ((BlackboardField)element).text = newValue;
        //    };
        //    blackboard.SetPosition(new Rect(10, 30, 200, 300));
        //    _graphView.Add(blackboard);
        //    _graphView.Blackboard = blackboard;
        //}

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

        #region node

        private DialogueEditorNode GenEntryPointNode()
        {
            DialogueEditorNode node = new DialogueEditorNode
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
            DialogueEditorNode node = new DialogueEditorNode
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddDialogueNode(DropdownMenuAction obj)
        {
            var n = obj.name.Split("/");
            // 1. ����Node
            DialogueEditorNode node = new DialogueEditorNode { 
                title = n[1], 
                GUID = Guid.NewGuid().ToString(), 
                Text = "Node", 
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

            // TODO:��ȡ�����λ��
            var pos = obj.eventInfo.mousePosition;
            var rect = new Rect(pos.x, pos.y, 100, 150);
            currentRect = rect;
            node.SetPosition(rect);

            AddElement(node);
        }

        private void AddOutputPort(DialogueEditorNode node)
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

        #endregion
    }
}