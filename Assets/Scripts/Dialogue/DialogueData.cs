using System.Collections.Generic;
using UnityEngine;

namespace Shel.Dialogue
{
    /// <summary>
    /// 节点类型
    /// </summary>
    public enum DialogueNodeType
    {
        /// <summary>
        /// 纯文本
        /// </summary>
        Text,
        /// <summary>
        /// 文本+头像
        /// </summary>
        TextWithAvatar,
        /// <summary>
        /// 选项
        /// </summary>
        Option,
        /// <summary>
        /// 自定义事件
        /// </summary>
        CustomEvent
    }

    /// <summary>
    /// 剧情所需的数据
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Dialogue/Create Dialogue")]
    public class DialogueData : ScriptableObject
    {
        public List<DialogueNode> Nodes;

        public void AddNode(DialogueNode node)
        {
            Nodes.Add(node);
        }

        public void RemoveNode(DialogueNode node)
        {
            Nodes.Remove(node);
        }
    }

    /// <summary>
    /// 存放数据用的结构
    /// data
    /// </summary>
    [System.Serializable]
    public class DialogueNode{
        /// <summary>
        /// 节点类型
        /// </summary>
        public DialogueNodeType Type;
        /// <summary>
        /// 头像
        /// TODO:拓展成角色的
        /// </summary>
        public Sprite Avatar;
        /// <summary
        /// 角色名
        /// </summary>
        public string CharacterName;
        // /// <summary>
        // /// 是否为主角
        // /// </summary>
        // public bool isProtagonist;
        /// <summary>
        /// 剧情文本
        /// </summary>
        public string Sentence;
        /// <summary>
        /// 选项标题（建议抽象）
        /// </summary>
        public string OptionTitle;
        /// <summary>
        /// 选项
        /// </summary>
        public List<DialogueOption> Options;
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName;
        /// <summary>
        /// 事件参数
        /// </summary>
        public List<DialogueEventArg> EventArgs;
    }


    /// <summary>
    /// 选项的内容
    /// </summary>
    [System.Serializable]
    public class DialogueOption{
        public string Title;
        public DialogueData Data;
    }

    /// <summary>
    /// 剧情事件
    /// TODO:待优化
    /// </summary>
    [System.Serializable]
    public class DialogueEventArg{
        public string EventKey;
        public string EventValue;
    }
}
