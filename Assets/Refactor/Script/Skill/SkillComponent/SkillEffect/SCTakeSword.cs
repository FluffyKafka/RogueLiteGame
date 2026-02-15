using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/Effects/Take Sword Effect")]
    internal class SCTakeSword : SCEffectBase
    {
        protected SMSword sword;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            sword = TryGetModel<SMSword>(_modelManager);
        }

        public override void Effect(string _id)
        {
            base.Effect(_id);
            sword.TakeSword();
        }
    }
}

