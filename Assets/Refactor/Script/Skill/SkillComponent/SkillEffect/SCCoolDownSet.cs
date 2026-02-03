using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/Effects/CooldownSet")]
    internal class SCCoolDownSet : SCEffectBase
    {
        protected SMTimer timer;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            timer = TryGetModel<SMTimer>(_modelManager);
        }

        public override void Effect(string _id)
        {
            base.Effect(_id);
            timer.SetTimer(_id);
        }
    }
}
