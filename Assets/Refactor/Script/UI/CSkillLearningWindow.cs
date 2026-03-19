using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CSkillLearningWindow : CUIComponentBase
    {
        [SerializeField] protected List<SLSkillLearningSlot> slots;
        [SerializeField] protected CSkillDetailToolTip skillDetailTooltip;

        Action<DSkillDetail> ShowSkillDetailNotice;
        Action HideSkillDetailNotice;
        Action SkillUnlockFinishNotice;

        protected override void Awake()
        {
            base.Awake();           
        }

        public void SetSkillLearningWindow(List<DSkillForSaleToUi> _skills)
        {
            ShowSkillDetailNotice += ShowSkillDetail;
            HideSkillDetailNotice += HideSkillDetail;
            SkillUnlockFinishNotice += SkillUnlockFinish;
            if (_skills != null)
            {
                for (int i = 0; i < _skills.Count; ++i)
                {
                    slots[i].SetSkill(_skills[i], ShowSkillDetailNotice, HideSkillDetailNotice, SkillUnlockFinishNotice);
                }
                gameObject.SetActive(true);
            }
            else
            {
                ShowSkillDetailNotice -= ShowSkillDetail;
                HideSkillDetailNotice -= HideSkillDetail;
                SkillUnlockFinishNotice -= SkillUnlockFinish;
                foreach(var slot in slots)
                {
                    slot.Hide();
                }
                gameObject.SetActive(false);
            }
        }

        protected void ShowSkillDetail(DSkillDetail _detail)
        {
            skillDetailTooltip.ShowDetail(_detail);
        }
        protected void HideSkillDetail()
        {
            skillDetailTooltip.HideToolTip();
        }
        protected void SkillUnlockFinish()
        {
            SetSkillLearningWindow(null);
            ui.NPCEffectFinish();
        }
    }
}

