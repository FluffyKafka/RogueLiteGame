using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/UnlockEffect/UnlockSubSkill")]
    internal class SCUnlockSubSkill : SCUnlockEffectBase
    {
        [SerializeField] protected SESkillEntity subSkill;

        public override void Effect()
        {
            base.Effect();
            subSkill.TryUnlock();
        }
    }
}

