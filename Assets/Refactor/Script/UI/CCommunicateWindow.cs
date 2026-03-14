using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class CCommunicateWindow : CUIComponentBase, IPointerClickHandler
    {
        [SerializeField] TextMeshProUGUI speakerName;
        [SerializeField] Image speakerIcon;
        [SerializeField] TextMeshProUGUI content;

        protected IDialog dialog = null;
        protected int sentenceIndex = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        public void ShowCommunicate(IDialog _dialog)
        {
            if(_dialog != null)
            {
                dialog = _dialog;
                sentenceIndex = 0;
                ShowSentence();
                gameObject.SetActive(true);
                ui.PauseGame(true);
            }
            else
            {
                dialog = null;
                sentenceIndex = 0;
                gameObject.SetActive(false);
                ui.PauseGame(false);
            }
        }

        public void OnPointerClick(PointerEventData _eventData)
        {            
            if(dialog == null)
            {
                return;
            }

            if(sentenceIndex + 1 < dialog.CheckDialog().Count)
            {
                ++sentenceIndex;
                ShowSentence();
            }
            else
            {
                CommunicateFinish();
            }
        }
        protected void ShowSentence()
        {
            List<IDialog.DSentence> sentences = dialog.CheckDialog();
            IDialog.DSentence sentence = sentences[sentenceIndex];
            IUIDialogEntity entity = dialog.CheckEntityByIndex(sentence.dialogEntityIndex).GetComponent<IUIDialogEntity>();
            speakerName.text = entity.CheckName();
            speakerIcon.sprite = entity.CheckIcon();
            content.text = sentence.text;
        }

        protected void CommunicateFinish()
        {
            ui.CommunicateFinish();
            ShowCommunicate(null);
        }
    }
}

