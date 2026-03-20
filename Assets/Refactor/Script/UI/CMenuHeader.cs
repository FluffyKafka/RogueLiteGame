using System.Collections;
using System.Collections.Generic;
using UIData;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CMenuHeader : CUIComponentBase
    {
        [SerializeField] protected Button toCharactor;
        [SerializeField] protected Button toCraft;
        [SerializeField] protected Button toSkill;
        [SerializeField] protected Button toMap;
        [SerializeField] protected Button toOption;
        protected bool isInit = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            if(!isInit)
            {
                toCharactor.onClick.AddListener(() => ChangePageTo(EUIPageType.Charactor));
                toCraft.onClick.AddListener(() => ChangePageTo(EUIPageType.Craft));
                toSkill.onClick.AddListener(() => ChangePageTo(EUIPageType.Skill));
                toMap.onClick.AddListener(() => ChangePageTo(EUIPageType.Map));
                toOption.onClick.AddListener(() => ChangePageTo(EUIPageType.Option));

                toCharactor.onClick.AddListener(() => ui.PlayButtonClickSFX(true));
                toCraft.onClick.AddListener(() => ui.PlayButtonClickSFX(true));
                toSkill.onClick.AddListener(() => ui.PlayButtonClickSFX(true));
                toMap.onClick.AddListener(() => ui.PlayButtonClickSFX(true));
                toOption.onClick.AddListener(() => ui.PlayButtonClickSFX(true));
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            toCharactor.onClick.RemoveAllListeners();
            toCraft.onClick.RemoveAllListeners();
            toSkill.onClick.RemoveAllListeners();
            toMap.onClick.RemoveAllListeners();
            toOption.onClick.RemoveAllListeners();
        }

        protected void ChangePageTo(EUIPageType _type)
        {
            ui.InvokeAction(ui.ChangePageTo, _type);
        }
    }
}
