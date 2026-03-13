using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLSkillCooldownSlot : CUIComponentBase
    {
        [SerializeField] protected Image skillIcon;
        [SerializeField] protected Image skillCooldownIcon;
        [SerializeField] protected TextMeshProUGUI inputKeyText;

        protected IUISkill skill;

        protected override void Awake()
        {
            base.Awake();
        }

        protected void Update()
        {
            if(skill != null)
            {
                skillCooldownIcon.fillAmount = skill.CheckCooldownPercent();
            }         
        }

        public void SetSkill(IUISkill _skill)
        {
            skill = _skill;
            ui = GetComponentInParent<MUIManager>();
            skillIcon.sprite = skill.CheckIcon();
            skillCooldownIcon.sprite = skill.CheckIcon();
            inputKeyText.text = ui.CheckSkillInputSlotKey(skill.CheckInputIndex()).ToString();
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            skill = null;
            gameObject.SetActive(false);
        }

        public IUISkill CheckSlotSkill()
        {
            return skill;
        }
    }
}

