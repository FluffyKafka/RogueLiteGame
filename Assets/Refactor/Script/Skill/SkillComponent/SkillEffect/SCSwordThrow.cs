using SkillSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/Effects/ThrowSwordEffect")]
    internal class SCSwordThrow : SCEffectBase
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
            sword.ThrowSword();
        }
    }
}

