using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/Unlock Condition/NoConflictSkill")]
    internal class SCSkillConflict : SCUnlockConditionBase
    {
        [SerializeField] List<SESkillEntity> conflictSkills;

        public override bool CanUnlock()
        {
            foreach(var skill in conflictSkills)
            {
                if(skill.IsUnlock())
                {
                    return false;
                }
            }
            return true;
        }
    }
}

