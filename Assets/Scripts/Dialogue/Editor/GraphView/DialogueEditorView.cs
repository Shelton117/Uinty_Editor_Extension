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
        /// 右键菜单栏
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // 如果有选中节点就显示基础的右键菜单
            // base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Text/Dialogue Node", AddDialogueNode);
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

        void AddManipulators()
        {
            // 允许拖拽Content
            this.AddManipulator(new ContentDragger());
            // 允许Selection里的内容
            this.AddManipulator(new SelectionDragger());
            // GraphView允许进行框选
            this.AddManipulator(new RectangleSelector());
            // 点击
            this.AddManipulator(new ClickSelector());
        }

        void InitializeView()
        {
            // 加载USS配置文件
            styleSheets.Add(Resources.Load<StyleSheet>("GraphView"));

            // 绘制网格
            GridBackground gridBackground = new GridBackground();
            gridBackground.name = "GridBackground";
            Insert(0, gridBackground);

            // 设置区域缩放范围
            SetupZoom(0.5f, 2.0f);

            this.StretchToParentSize();

            // 其他界面绘制
            // 显示当前缩放大小

            // 缩略图
            GenerateMiniMap();
            // 搜索框
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
            DialogueEditorNode node = new DialogueEditorNode
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddDialogueNode(DropdownMenuAction obj)
        {
            var n = obj.name.Split("/");
            // 1. 创建Node
            DialogueEditorNode node = new DialogueEditorNode { 
                title = n[1], 
                GUID = Guid.NewGuid().ToString(), 
                Text = "Node", 
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

            // TODO:获取鼠标点击位置
            var pos = obj.eventInfo.mousePosition;
            var rect = new Rect(pos.x, pos.y, 100, 150);
            currentRect = rect;
            node.SetPosition(rect);

            AddElement(node);
        }

        private void AddOutputPort(DialogueEditorNode node)
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

        #endregion
    }
}