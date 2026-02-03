using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMPlayer : SMSkillModel
    {
        public bool CanEffectBehaviourSkill()
        {
            return manager.CanEffectBehaviourSkill();
        }
    }
}