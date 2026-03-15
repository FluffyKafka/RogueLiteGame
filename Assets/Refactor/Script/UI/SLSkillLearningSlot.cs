using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLSkillLearningSlot : CUIComponentBase, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] protected Image skillIcon;
        protected Action<DSkillDetail> ShowSkillDetail;
        protected Action HideSkillDetail;
        protected Action SkillUnlockFinish;

        protected DSkillDetail skillDetail = new();
        protected IUISkill skill;

        public void SetSkill(DSkillForSaleToUi _skill, Action<DSkillDetail> _showDetailAction, Action _hideDetailAction, Action _skillUnlockFinishAction)
        {
            skillIcon.sprite = _skill.skill.CheckIcon();
            ShowSkillDetail = _showDetailAction;
            HideSkillDetail = _hideDetailAction;
            SkillUnlockFinish = _skillUnlockFinishAction;
            skill = _skill.skill;

            skillDetail.skillName = _skill.skill.CheckName();
            skillDetail.icon = _skill.skill.CheckIcon();
            skillDetail.description = _skill.skill.CheckDescription();
            skillDetail.price = _skill.price;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowSkillDetail.Invoke(skillDetail);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            HideSkillDetail.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ui.ConsumeSoul(skillDetail.price);
            bool isSuccess = skill.TryUnlock();
            SkillUnlockFinish.Invoke();
        }
    }
}

