namespace Shel.Dialogue {
    /// <summary>
    /// 剧情界面（接口）
    /// </summary>
    public interface IDialogueView
    {
        /// <summary>
        /// 界面控制器
        /// </summary>
        /// <value>子类对象赋值</value>
        IDialogueController Controller { set; get; }
        /// <summary>
        /// 界面初始化
        /// </summary>
        void OnViewInit();
        /// <summary>
        /// 剧情开始
        /// </summary>
        void OnDialogueStart();
        /// <summary>
        /// 剧情数据节点开始
        /// </summary>
        /// <param name="node"></param>
        void OnDialogueNodeStart(DialogueNode node, int index);
        /// <summary>
        /// 完成剧情
        /// </summary>
        void OnDialogueFinish();
    }
}