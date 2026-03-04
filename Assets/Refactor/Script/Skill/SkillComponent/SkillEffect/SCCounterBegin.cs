using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/Effects/BeginCounter")]
    internal class SCCounterBegin : SCEffectBase
    {
        protected SMCounterAttack counter;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            counter = TryGetModel<SMCounterAttack>(_modelManager);
        }

        public override void Effect(string _id)
        {
            base.Effect(_id);
            counter.BeginCounter();
        }
    }
}

