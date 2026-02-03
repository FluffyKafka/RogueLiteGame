using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectCondition/BehaviourSkillCheck")]
    internal class SCBehaviourSkillCheck : SCEffectConditionBase
    {
        protected SMPlayer player;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            player = TryGetModel<SMPlayer>(_modelManager);
        }

        public override bool CanEffect(string _id)
        {
            return player.CanEffectBehaviourSkill();
        }
    }
}

