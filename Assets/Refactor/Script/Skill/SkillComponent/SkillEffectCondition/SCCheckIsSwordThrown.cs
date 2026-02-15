using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectCondition/Can Throw Sword")]
    internal class SCCheckIsSwordThrown : SCEffectConditionBase
    {
        [SerializeField] protected bool CanEffectIfNotThrow = true;

        protected SMSword sword;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            sword = TryGetModel<SMSword>(_modelManager);
        }

        public override bool CanEffect(string _id)
        {
            if(CanEffectIfNotThrow)
            {
                return sword.IsSwordThrown();
            }
            else
            {
                return !sword.IsSwordThrown();
            }
        }
    }

}
