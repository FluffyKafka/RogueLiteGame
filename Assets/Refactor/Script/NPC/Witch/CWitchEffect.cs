using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class CWitchEffect : CNPCEffectBase
    {
        [SerializeField] protected int maxOptionAmount;
        [SerializeField] protected float priceMultiplier;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Effect()
        {
            base.Effect();
            float soul = npc.CheckPlayerSoulAmount();
            List<ScriptableObject> skills = npc.CheckPlayerCanUnlockSkillList(soul / priceMultiplier);
            List<DSkillForSaleToUi> skillOptions = new(maxOptionAmount);
            for(int i = 0; i < maxOptionAmount; ++i)
            {
                if(skills.Count == 0)
                {
                    if(skillOptions.Count == 0)
                    {
                        npc.InvokeAction(npc.EffectFailNotice);
                        return;
                    }
                    break;
                }
                ScriptableObject skill = skills[Random.Range(0, skills.Count)];
                skills.Remove(skill);
                skillOptions.Add(new DSkillForSaleToUi(skill as IUISkill, (skill as IUISkill).CheckPrice() * priceMultiplier));
            }
            npc.ShowSkillForSaleListToPlayer(skillOptions);
        }
    }
}

