using System;
using System.Collections;
using UnityEngine;

namespace Shel.Dialogue
{
    /// <summary>
    /// 文本播放器
    /// 控制对话框内的文本逐字显示
    /// </summary>
    public class TextPlayer
    {
        /// <summary>
        /// 播放器缓存文本内容
        /// </summary>
        string mCurrentSentence = string.Empty;
        /// <summary>
        /// 完成播放的事件（缓存）
        /// </summary>
        Action OnFinish;

        /// <summary>
        /// 开始播放对话内容
        /// </summary>
        /// <param name="sentence">对话内容</param>
        /// <param name="OnPlayText">需要播放的文本</param>
        /// <param name="onFinish">内容播放完成后触发的事件</param>
        /// <returns></returns>
        public IEnumerator StartPlaying(string sentence,
            Action<string> OnPlayText,
            Action onFinish = null)
        {
            mCurrentSentence = sentence;
            OnFinish = onFinish;

            var sentenceToPlay = string.Empty;
            var lenght = mCurrentSentence.Length;

            for (int i = 0; i < lenght; i++)
            {
                yield return new WaitForSeconds(0.1f);

                sentenceToPlay = mCurrentSentence.Substring(0, i);

                OnPlayText?.Invoke(sentenceToPlay);
            }

            yield return new WaitForSeconds(0.1f);

            sentenceToPlay = mCurrentSentence;

            OnPlayText?.Invoke(sentence);

            Finish();
        }

        /// <summary>
        /// 对话完成，触发事件
        /// </summary>
        public void Finish(){
            OnFinish?.Invoke();
            OnFinish = null;
        }
    }
}
