using System;
using System.Collections.Generic;

namespace Shel.Dialogue
{
    /// <summary>
    /// 剧情（控制器）状态
    /// </summary>
    public enum DialogueState{
        No_Start,
        Started,
        Paused,
        Auto,
    }

    /// <summary>
    /// 剧情控制器
    /// </summary>
    public class DialogueController : IDialogueController
    {
        /// <summary>
        /// 剧情（控制器）状态
        /// </summary>
        /// <value>（默认）未开始</value>
        public DialogueState State { get; private set; } = DialogueState.No_Start;
        /// <summary>
        /// 进行的节点序号
        /// </summary>
        /// <value>从0开始</value>
        public int CurrentNodeIndex { get; private set; } = 0;

        /// <summary>
        /// 全局默认对话控制器
        /// </summary>
        /// <returns>（返回）控制器本身</returns>
        public static readonly DialogueController Default = new DialogueController();
        /// <summary>
        /// 记录剧情事件
        /// </summary>
        public event Action<string, List<DialogueEventArg>> OnCustomEvent = (s, list) => { };
        /// <summary>
        /// 翻译器 <中文，外文>
        /// </summary>
        public Func<string, string> Translator = text => text;

        /// <summary>
        /// 执行效果的数据
        /// </summary>
        private DialogueData mDialogueData;

        #region 对外函数

        /// <summary>
        /// 控制器与界面关联
        /// </summary>
        /// <param name="view">用户界面</param>
        public void ConnectView(IDialogueView view){
            View = view;
            View.Controller = this;
        }

        /// <summary>
        /// 取消关联
        /// </summary>
        public void DisConnectView(){
            View.Controller = null;
            View = null;
        }

        /// <summary>
        /// 设置剧情数据
        /// 显示的内容以这里的为准，view层为缓存
        /// </summary>
        /// <param name="data"></param>
        public void SetData(DialogueData data){
            mDialogueData = data;
        }

        #endregion

        #region 实现接口函数

        public bool StepEnable { get; set; } = true;

        public IDialogueView View { get; private set; }

        public void Init(){
            State = DialogueState.No_Start;
            CurrentNodeIndex = 0;
            View?.OnViewInit();
        }

        public void Step(){
            if (!StepEnable) return;

            switch (State)
            {
                case DialogueState.No_Start:
                    State = DialogueState.Started;
                    CurrentNodeIndex = 0;
                    View.OnDialogueStart();
                    break;
                case DialogueState.Started:
                    if (mDialogueData.Nodes.Count <= CurrentNodeIndex)
                    {
                        State = DialogueState.No_Start;
                        View.OnDialogueFinish();
                    }
                    else
                    {
                        // 播放节点内容
                        var node = mDialogueData.Nodes[CurrentNodeIndex];
                        View?.OnDialogueNodeStart(node, CurrentNodeIndex);
                    }
                    break;
                default:
                    break;
            }
        }

        public void NextStep(){
            CurrentNodeIndex++;
            Step();
        }

        public void StepAtFirst(){
            CurrentNodeIndex = 0;
            Step();
        }

        public void MoveToNext(){
            CurrentNodeIndex++;
        }

        public void TriggerCustomEvent(string eventName, List<DialogueEventArg> args){
            OnCustomEvent?.Invoke(eventName, args);
        }

        public string Translate(string text){
            return Translator(text);
        }

        #endregion
    }
}
