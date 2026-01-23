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

        protected override void OnEnable()
        {
            base.OnEnable();

            toCharactor.onClick.AddListener(() => ChangePageTo(EUIPageType.Charactor));
            toCraft.onClick.AddListener(() => ChangePageTo(EUIPageType.Craft));
            toSkill.onClick.AddListener(() => ChangePageTo(EUIPageType.Skill));
            toMap.onClick.AddListener(() => ChangePageTo(EUIPageType.Map));
            toOption.onClick.AddListener(() => ChangePageTo(EUIPageType.Option));
        }

        protected void ChangePageTo(EUIPageType _type)
        {
            ui.InvokeAction(ui.ChangePageTo, _type);
        }
    }
}
