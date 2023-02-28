using System.Collections.Generic;

namespace Shel.Dialogue
{
    /// <summary>
    /// 剧情控制器（接口）
    /// 控制输入更改界面状态
    /// </summary>
    public interface IDialogueController
    {
        /// <summary>
        /// 步骤（启用）
        /// </summary>
        /// <value>否（默认）</value>
        bool StepEnable { set; get; }
        /// <summary>
        /// 用户界面
        /// </summary>
        /// <value>子类对象赋值</value>
        IDialogueView View { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 步骤
        /// </summary>
        void Step();
        /// <summary>
        /// 下一步
        /// </summary>
        void NextStep();
        /// <summary>
        /// 回到第一步
        /// </summary>
        void StepAtFirst();
        /// <summary>
        /// 执行到下一步？
        /// </summary>
        void MoveToNext();
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">事件列表</param>
        void TriggerCustomEvent(string eventName, List<DialogueEventArg> args);
        /// <summary>
        /// 翻译文本
        /// </summary>
        /// <param name="text">被翻译的文本</param>
        /// <returns></returns>
        string Translate(string text);
    }
}