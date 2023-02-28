using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shel.Dialogue
{
    /// <summary>
    /// 剧情对话的用户界面
    /// </summary>
    public class DialogueView : MonoBehaviour, IDialogueView
    {
        #region 变量
        /// <summary>
        /// 仅用于面板缓存
        /// 实际读取的数据位于Controller
        /// </summary>
        /// <returns></returns>
        [SerializeField] DialogueData dialogueData;
        [SerializeField] Image avatar;
        [SerializeField] Text dialogueText, optionTitle, characterName;
        [SerializeField] GameObject optionRoot;
        [SerializeField] GameObject optionItem;
        [SerializeField] GameObject textBoxBG;
        TextPlayer textPlayer = new TextPlayer();
        /// <summary>
        /// 携程
        /// </summary>
        Coroutine textPlayerCoroutine = null;
        DialogueController controller;
        [SerializeField] AudioSource sound;

        #endregion

        #region 生命周期

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            controller = DialogueController.Default;
            controller.ConnectView(this);
            controller.SetData(dialogueData);
            controller.Init();
            controller.OnCustomEvent += OnDialogueCustomEvent;
            controller.Translator = (text) =>{
                // 多语言？
                return text;
            };

            Controller.Step();
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            controller.Translator = (text) => text;
            controller.DisConnectView();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            // 判断下当前的节点是不是选项节点
            if (Input.GetMouseButtonDown(0))
            {
                Play();
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {

        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        void OnDisable()
        {
            controller.OnCustomEvent -= OnDialogueCustomEvent;
        }

        #endregion

        #region 实现接口方法

        public IDialogueController Controller { set; get; }

        public void OnViewInit(){
            Debug.Log(dialogueData.name + ": View Init");

            Finish();
        }

        public void OnDialogueStart(){
            Debug.Log(dialogueData.name + ": Dialogue Start");
        }

        public void OnDialogueNodeStart(DialogueNode node, int index){
            Debug.Log(index + ": Dialogue Node Start");

            Finish();

            switch (node.Type)
            {
                case DialogueNodeType.Text:
                    SetText(node);
                    break;
                case DialogueNodeType.TextWithAvatar:
                    SetTextWithAvatar(node);
                    break;
                case DialogueNodeType.Option:
                    SetOption(node);
                    break;
                case DialogueNodeType.CustomEvent:
                    SetCustomEvent(node);
                    break;
            }
        }

        public void OnDialogueFinish(){
            Debug.Log("Dialogue Finish");

            // 播放结束后删除ui
            Finish();
            Destroy(this.gameObject);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 意义不明？
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        void OnDialogueCustomEvent(string eventName, List<DialogueEventArg> args){
            Debug.Log(eventName + ":" +
                args.Find(arg => arg.EventKey == "Count").EventValue);
        }

        void Play(){
            if (textPlayerCoroutine != null)
                {
                    StopCoroutine(textPlayerCoroutine);
                    textPlayer.Finish();
                }
                else
                {
                    Controller.Step();
                }
        }

        void Finish(){
            avatar.gameObject.SetActive(false);

            dialogueText.gameObject.SetActive(false);
            optionTitle.gameObject.SetActive(false);
            characterName.gameObject.SetActive(false);

            optionRoot.SetActive(false);
            optionItem.SetActive(false);
            textBoxBG.SetActive(false);

            dialogueText.text = string.Empty;
            optionTitle.text = string.Empty;
            characterName.text = string.Empty;
        }

        void SetText(DialogueNode node){
            dialogueText.gameObject.SetActive(true);
            characterName.gameObject.SetActive(true);
            textBoxBG.SetActive(true);

            dialogueText.text = string.Empty;
            characterName.text = node.CharacterName;

            Controller.StepEnable = false;

            textPlayerCoroutine = StartCoroutine(
                textPlayer.StartPlaying(
                    Controller.Translate(node.Sentence),
                    (text) =>
                    {
                        dialogueText.text = text;

                        sound.Play();
                    },
                    () =>
                    {
                        dialogueText.text = Controller.Translate(node.Sentence);

                        Controller.StepEnable = true;
                        Controller.MoveToNext();
                        textPlayerCoroutine = null;
                        sound.Play();
                    }
                )
            );
        }

        void SetTextWithAvatar(DialogueNode node){
            avatar.gameObject.SetActive(true);
            avatar.sprite = node.Avatar;

            SetText(node);
        }

        void SetOption(DialogueNode node){
            // 显示选项
            optionTitle.gameObject.SetActive(true);
            optionRoot.SetActive(true);

            ClearRoot();

            optionTitle.text = string.Empty;
            Controller.StepEnable = false;

            textPlayerCoroutine = StartCoroutine(
                textPlayer.StartPlaying(
                    Controller.Translate(
                    node.OptionTitle
                    ),
                    (text) => {
                        optionTitle.text = text;

                        sound.Play();
                    },
                    () => {
                        optionTitle.text = Controller.Translate(node.OptionTitle);
                        textPlayerCoroutine = null;

                        sound.Play();

                        foreach (var option in node.Options)
                        {
                            var cachedDialogueOption = option;
                            var title = Controller.Translate(cachedDialogueOption.Title);
                            var item = Instantiate(optionItem, optionRoot.transform);

                            item.SetActive(true);
                            item.GetComponentInChildren<Text>().text = title;
                            item.GetComponent<Button>().onClick.AddListener(
                                () =>
                                {
                                    Controller.StepEnable = true;

                                    if (cachedDialogueOption.Data)
                                    {
                                        dialogueData = cachedDialogueOption.Data;
                                        DialogueController.Default.SetData(dialogueData);
                                        this.Controller.StepAtFirst();
                                    }
                                    else
                                    {
                                        Controller.NextStep();
                                    }
                                }
                            );
                        }
                    }
                )
            );
        }

        void SetCustomEvent(DialogueNode node){
            Controller.TriggerCustomEvent(node.EventName, node.EventArgs);
            Controller.NextStep();
        }

        /// <summary>
        /// 复原节点
        /// 清除root下的选项
        /// </summary>
        void ClearRoot(){
            var children = optionRoot.GetComponentsInChildren<Button>();
            foreach (var chil in children)
            {
                if (chil.gameObject != optionItem)
                {
                    Destroy(chil.gameObject);
                }
            }
        }

        #endregion

        #region 对外方法

        public void SetData(DialogueData data){
            dialogueData = data;

            if (controller != null)
            {
                controller.SetData(dialogueData);
            }
        }

        #endregion
    }
}