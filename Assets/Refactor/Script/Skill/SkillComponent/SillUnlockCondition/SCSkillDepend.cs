using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/Unlock Condition/HaveDependSkill")]
    internal class SCSkillDepend : SCUnlockConditionBase
    {
        [SerializeField] List<SESkillEntity> dependSkills;

        public override bool CanUnlock()
        {
            foreach(var skill in dependSkills)
            {
                if(!skill.IsUnlock())
                {
                    return false;
                }
            }
            return true;
        }

        public List<string> CheckDependIds()
        {
            List<string> res = new();
            foreach(SESkillEntity skill in dependSkills)
            {
                res.Add(skill.CheckId());
            }
            return res;
        }
    }
}

