using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace UISystem
{
    internal class CSkillCooldownList : CUIComponentBase
    {
        [SerializeField] protected List<SLSkillCooldownSlot> skillSlots;

        protected int tailIndex = 0;
        protected bool isAwaking = false;

        protected override void Awake()
        {
            base.Awake();
            ui.SkillUnlockNotice += SkillUpdate;
            isAwaking = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (isAwaking)
            {
                isAwaking = false;
                return;
            }
            else
            {               
                UpdateSkillCooldownList();
            }
        }
        protected void UpdateSkillCooldownList()
        {
            List<IUISkill> skills = ui.CheckSkillsUnlockedHaveCooldown();
            Debug.Log(skills.Count);
            for (int i = 0; i < skills.Count; ++i)
            {
                skillSlots[i].SetSkill(skills[i]);
                ++tailIndex;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var slot in skillSlots)
            {
                slot.Clear();
            }
            tailIndex = 0;
        }

        protected void SkillUpdate(IUISkill _skill)
        {
            if(!_skill.IsSkillHaveCooldown())
            {
                return;
            }

            if(tailIndex >= skillSlots.Count)
            {
                Debug.LogWarning("槽位不足无法显示");
                return;
            }

            if(skillSlots.Find((slot) => slot.CheckSlotSkill() == _skill) != null)
            {
                return;
            }

            skillSlots[tailIndex].SetSkill(_skill);
            ++tailIndex;
        }
    }
}

